-- add fields: priority, reminder_date to the todo table

ALTER TABLE IF EXISTS app.todos
    ADD COLUMN IF NOT EXISTS priority SMALLINT DEFAULT 0;

ALTER TABLE IF EXISTS app.todos
    ADD COLUMN IF NOT EXISTS reminder_date TIMESTAMP;
