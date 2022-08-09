using System;

namespace VisualFindReferences.Core.Graph.Model
{
    public class ProjectModel : ModelBase
    {
        public ProjectModel(string projectName, bool isIncludedInSearches, Guid projectId)
        {
            ProjectName = projectName;
            _isIncludedInSearches = isIncludedInSearches;
            ProjectId = projectId;
        }

        public string ProjectName { get; }

        public Guid ProjectId { get; } 

        private bool _isIncludedInSearches;

        public bool IsIncludedInSearches
        {
            get { return _isIncludedInSearches; }
            set
            {
                if (value != _isIncludedInSearches)
                {
                    _isIncludedInSearches = value;
                    RaisePropertyChanged(nameof(IsIncludedInSearches));
                }
            }
        }
    }
}