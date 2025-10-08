# CQRS - Command and Query Responsibility Segregation

The idea of the CQRS is to divide the reads and writes to a database in two.

You should have a clear separation of commands (writes) and queries (reads).

Another common way of implementing CQRS is to have 2 different databases. One
for the reads and one for the writes. So you can have data in different formats
in each one. Like in write database you have the standard tables for your
entities and their relations and in read database you can have a table for
the information for a page in a single table without needing to make lots
of joins and complicated queries.

With 2 databases you can also have 1 postgresql for writes and 1 mongo db for reads
in the postgresql you do the relational storage, in the mongo db instead of having
the relational tables you can store the views (like sql view with lots of table
joins) ready to be consumed for the application frontend.

One of the advantages is having 2 dbs is so you can optimize the query db only for
queries using a lot of caching. One good model is also using a normal db for commands
and a caching as a query db.

Another advantage of the separation is to maintain an backup db and logs. You dont
need any logic about backup and logs in queries and can have it only for commands.

## Event Sourcing

You save not only the normal information of your app. You also saves the diffs that
the commands do. So the database may be able to recreate itself from zero to the
current state if you applied the diffs in order. Very similar to what git does with
the commits. This can work both as logging and backup.

The events can be the single source of true and the database will act like a
snapshot of one of this events, so you don't have to process the events every time
you want some information.

One ideia of having the events to act like a source of true is that you can go back
in time in your database just be reverting the events in order. Like you had some
problem and the data from the last 3 days go wrong values because of a bug and the
database base got all messed up. Up can revert the events from the last 3 days and
have the database state of 3 day ago.

This is not that new. The banks already do it. They dont just register the amount
of money the accounts have. They also saves trasferences, deposits and withdraws,
those are events with meta information.

The ideia of event sourcing is to make the changes to data to be registered as
diffs that can be applied or reverted at will but with enough information that
they can be chronologically ordered.
