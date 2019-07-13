namespace FamilyGuy.Infrastructure.Communication.Interfaces
{
    public interface IQuery
    {
        TQueryResponse Query<TQueryResponse, TQueryRequest>(TQueryRequest request);
    }
}