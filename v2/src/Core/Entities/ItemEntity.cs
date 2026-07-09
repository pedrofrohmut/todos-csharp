using Todos.Core.Utils;
using Todos.Core.Errors;
using Todos.Core.Commands;
using Todos.Core.Commands.Handlers;

namespace Todos.Core.Entities;

public class ItemEntity
{
    public static Result ValidateId(int id)
    {
        if (id < 1) {
            return Result.Fail(new InvalidItemError("Invalid item id. Id cannot be less than 1."));
        }
        return Result.Ok();
    }

    public static Result ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) {
            return Result.Fail(new InvalidItemError("Name not provided. Name is required and cannot be blank."));
        }
        if (name.Length < 3) {
            return Result.Fail(new InvalidItemError("Name is too short. Name must be at least 3 characters long."));
        }
        if (name.Length > 120) {
            return Result.Fail(new InvalidItemError("Name is too long. Name must be less than 121 characters long."));
        }
        if (!EntitiesUtils.IsValidName(name)) {
            return Result.Fail(new InvalidItemError("Name contains invalid characters."));
        }
        return Result.Ok();
    }

    public static Result ValidateDescription(string description)
    {
        if (description.Length < 3) {
            return Result.Fail(new InvalidItemError("Description is too short. Description must be at least 3 characters long."));
        }
        if (description.Length > 255) {
            return Result.Fail(new InvalidItemError("Description is too long. Description must be less than 256 characters long."));
        }
        return Result.Ok();
    }

    public static async Task<Result> CreateItem(ItemCreateCommand command, IItemCommandHandler handler)
    {
        try {
            await handler.CreateItem(command);
            return Result.Ok();
        } catch (Exception e) {
            return Result.Fail("Item:" + nameof(CreateItem), "Error to create item: " + e.Message);
        }
    }
}
