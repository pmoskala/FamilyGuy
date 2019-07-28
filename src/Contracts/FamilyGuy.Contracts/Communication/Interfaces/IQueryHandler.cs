namespace FamilyGuy.Contracts.Communication.Interfaces
{
    public interface IQueryHandler<out TQueryResponse, in TQueryRequest>
    {
        TQueryResponse Handle(TQueryRequest request);
    }
}