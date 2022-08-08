namespace VisualFindReferences.Core.Graph.Model
{
    public class ProjectModel : ModelBase
    {
        public ProjectModel(string projectName, bool isIncludedInSearches)
        {
            ProjectName = projectName;
            _isIncludedInSearches = isIncludedInSearches;
        }

        public string ProjectName { get; }

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