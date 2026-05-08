namespace Moving.Api.Contracts;

public record CreateStorageBoxRequest(string? Description);

public record UpdateStorageBoxRequest(string? Description);

public record CreateStoredItemRequest(
    string ItemName,
    string? ItemDescription,
    string? Keywords,
    int Quantity);

public record UpdateStoredItemRequest(
    string ItemName,
    string? ItemDescription,
    string? Keywords,
    int Quantity);

public record LocateItemRequest(string Search);
