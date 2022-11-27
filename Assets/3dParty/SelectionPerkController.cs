namespace ThirdParty
{
    public delegate void SelectionChangedDelegate<in T>(T old, T current); 
    public class Selection<T> where T: ISelection
    {
        public event SelectionChangedDelegate<T> SelectionChanged;
        private T _currentSelected;
        
        public void Select(T perkViewController)
        {
            if (_currentSelected != null)
            {
                _currentSelected.Deselect();
            }

            var old = _currentSelected;

            _currentSelected = perkViewController;
            _currentSelected.Select();
            SelectionChanged?.Invoke(old, _currentSelected);
        }
    }

    public interface ISelection
    {
        void Deselect();
        void Select();
    }
}