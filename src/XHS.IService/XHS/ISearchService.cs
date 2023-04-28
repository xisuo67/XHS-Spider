using XHS.IService.DI;

namespace XHS.IService.XHS
{
    public interface ISearchService: ISingletonDependency
    {
        void SearchInput(string input);
    }
}
