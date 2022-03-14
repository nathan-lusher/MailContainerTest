# Mail Container Test

The code for this exercise has been developed to manage the transfer of mail items from one container to another for processing.

## Process for transferring mail

- Lookup the container the mail is being transferred from.
- Check the containers are in a valid state for the transfer to take place.
- Reduce the container capacity on the source container and increase the destination container capacity by the same amount.

## Restrictions

- A container can only hold one type of mail.

## Assumptions

- For the sake of simplicity, we can assume the containers have an unlimited capacity.

# The exercise brief

The exercise is to take the code in the solution and refactor it into a more suitable approach with the following things in mind:

- Testability
- Readability
- SOLID principles
- Architectural design of the code

You should not change the method signature of the MakeMailTransfer method.

You should add suitable tests into the MailContainerTest.Test project.

There are no additional constraints, use the packages and approach you feel appropriate, aim to spend no more than 2 hours. Please update the readme with specific comments on any areas that are unfinished and what you would cover given more time.

# Implementation Notes

- Time taken: ~2.5 hours
- The structure of the solution has not been changed but, depending on the wider system, could be rearranged. Domain objects used by the wider system, for example, should be moved out into a separate namespace and / or assembly. For simplicity, everything has been kept in a single assembly.
- An invalid request has been considered an error scenario, so an exception is thrown, rather than returning a transfer failure status.
- Use (and abuse) of `var` is often debated but, in this implementation, is only used when the actual type is obvious.
- The `Capacity` property of the `MailContainer` object was renamed to `TotalItems` to be less ambiguous / confusing. The definition of the word "capacity" is "the maximum amount that something can contain" so it does not make sense to reduce the capacity when items are removed, as per the above instructions.
- Xml summaries have been added to public methods to provide information and intellisense to the downstream components. This could also be used to automate the building of API documentation.
- Basic Github actions were added to build and test the solution on pull-requests and pushes to the main branch.

## Omissions

Depending on requirements and time constraints, the following items could additionally be implemented:

- Proper implementation of `MailContainerDataStore` and `BackupMailContainerDataStore`.
- Logging - there is currently no longing within the application but would make debugging easier. This could be implemented by injecting an `ILogger`.
- Transfer dates - the `MailTransferRequest.TransferDate` is currently ignored but could be used to defer transfer of items until a later date. This could be implemented by adding requests to a queue for processing at a later date.
- Async operations - there are currently no asynchronous operations in the solution but could be implemented as required.
