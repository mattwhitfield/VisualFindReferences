using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using VisualFindReferences.Core.Graph.Model;

namespace VisualFindReferences.Core.Graph.ViewModel
{
    public class VFRNodeGraphViewModel : NodeGraphViewModel
    {
        public VFRNodeGraphViewModel(VFRNodeGraph nodeGraph) : base(nodeGraph)
        {
        }

        private Regex _projectFilterMatch = new Regex(@"(.*\.Tests|.*\.UnitTests)");

        private string _projectFilterMatchPattern = "*.Tests;*.UnitTests";

        public string ProjectFilterMatchPattern
        {
            get { return _projectFilterMatchPattern; }
            set
            {
                if (value != _projectFilterMatchPattern)
                {
                    _projectFilterMatchPattern = value;

                    // TODO - build actual regex

                    RaisePropertyChanged(nameof(ProjectFilterMatchPattern));
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
                    _excludedProjectIds.Clear();
                    foreach (var project in _lastSolution.Projects)
                    {
                        var isIncluded = !_projectFilterMatch.IsMatch(project.Name);
                        Projects.Add(new ProjectModel(project.Name, isIncluded));
                        if (!isIncluded)
                        {
                            _excludedProjectIds.Add(project.Id.Id);
                        }
                    }
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