using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using VisualFindReferences.Core.Graph.Model;

namespace VisualFindReferences.Core.Graph.ViewModel
{
    public class VFRNodeGraphViewModel : NodeGraphViewModel
    {
        public VFRNodeGraphViewModel(VFRNodeGraph nodeGraph) : base(nodeGraph)
        {
        }

        private Regex _projectFilterMatch = new Regex(@"(.*\.Tests|.*\.UnitTests)", RegexOptions.IgnoreCase);

        private string _projectFilterMatchPattern = "*.Tests;*.UnitTests";

        public string ProjectFilterMatchPattern
        {
            get { return _projectFilterMatchPattern; }
            set
            {
                if (value != _projectFilterMatchPattern)
                {
                    _projectFilterMatchPattern = value;

                    var segments = _projectFilterMatchPattern.Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => Regex.Escape(x.Trim()));
                    var regexSegments = segments.Select(x => "^" + x.Replace(@"\*", @".*").Replace(@"\?", ".") + "$").ToArray();
                    var regex = "(" + string.Join("|", regexSegments) + ")";
                    _projectFilterMatch = new Regex(regex, RegexOptions.IgnoreCase);
                    ApplyRegex();

                    RaisePropertyChanged(nameof(ProjectFilterMatchPattern));
                }
            }
        }

        private DoubleClickAction _doubleClickAction = DoubleClickAction.GoToCode;

        public DoubleClickAction DoubleClickAction
        {
            get { return _doubleClickAction; }
            set
            {
                if (value != _doubleClickAction)
                {
                    _doubleClickAction = value;
                    RaisePropertyChanged(nameof(DoubleClickAction));
                }
            }
        }

        private void ApplyRegex()
        {
            _excludedProjectIds.Clear();
            foreach (var project in Projects)
            {
                var isIncluded = !_projectFilterMatch.IsMatch(project.ProjectName);
                project.IsIncludedInSearches = isIncluded;
                if (!isIncluded)
                {
                    _excludedProjectIds.Add(project.ProjectId);
                }
            }
        }

        protected override void HandleContinuation<T>(T result)
        {
            if (result is FoundReferences foundReferences)
            {
                if (foundReferences.Solution != _lastSolution)
                {
                    _lastSolution = foundReferences.Solution;
                    Projects.Clear();
                    foreach (var project in _lastSolution.Projects)
                    {
                        Projects.Add(new ProjectModel(project.Name, true, project.Id.Id));
                    }
                    ApplyRegex();
                }
            }
        }

        private HashSet<Guid> _excludedProjectIds = new HashSet<Guid>();

        private Solution? _lastSolution;

        public ObservableCollection<ProjectModel> Projects { get; } = new ObservableCollection<ProjectModel>();

        public bool ShowFilteredReferencesPrompt { get; private set; }

        private string _filteredReferencesMessage = string.Empty;

        public string FilteredReferencesMessage
        {
            get { return _filteredReferencesMessage; }
            set
            {
                if (value != _filteredReferencesMessage)
                {
                    _filteredReferencesMessage = value;
                    ShowFilteredReferencesPrompt = !string.IsNullOrWhiteSpace(value);
                    RaisePropertyChanged(nameof(ShowFilteredReferencesPrompt));
                    RaisePropertyChanged(nameof(FilteredReferencesMessage));
                }
            }
        }

        internal Func<Project, bool> GetProjectFilter()
        {
            return x => !_excludedProjectIds.Contains(x.Id.Id);
        }
    }
}