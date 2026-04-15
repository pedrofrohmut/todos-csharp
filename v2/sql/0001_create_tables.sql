/*
    1. All tables should have created_at with the utc timestamp
    updated_at with the timestamp on updates

    2. The primary should be placed as the last line like: primary key (id)

    3. The foreign key should be like: fk_<this_table>_<ref_table>
        Example: constraint fk_todos_users foreign key (user_id) references users (id)

    4. Use VARCHAR with explicit length limits matching entity validation rules.

    5. Add a UNIQUE constraint on email to enforce uniqueness at the DB level.
 */

create table if not exists users (
    id serial not null,
    name varchar(120) not null,
    email varchar(255) not null,
    password_hash varchar(255) not null,
    created_at timestamp not null default (now() at time zone 'utc'),
    updated_at timestamp not null default (now() at time zone 'utc'),
    constraint uq_users_email unique (email),
    primary key (id)
);

create table if not exists todos (
    id serial not null,
    name varchar(120) not null,
    description varchar(255),
    user_id int not null,
    is_done boolean default false,
    created_at timestamp not null default (now() at time zone 'utc'),
    updated_at timestamp not null default (now() at time zone 'utc'),
    constraint fk_todos_users foreign key (user_id) references users (id),
    primary key (id)
);

create table if not exists todo_items (
    id serial not null,
    name varchar(120) not null,
    description varchar(255),
    todo_id int not null,
    user_id int not null,
    is_done boolean default false,
    created_at timestamp not null default (now() at time zone 'utc'),
    updated_at timestamp not null default (now() at time zone 'utc'),
    constraint fk_todo_items_todos foreign key (todo_id) references todos (id),
    constraint fk_todo_items_users foreign key (user_id) references users (id),
    primary key (id)
);
