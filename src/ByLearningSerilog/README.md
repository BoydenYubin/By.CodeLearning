### How to use [Serilog](https://serilog.net/) with [Seq](https://docs.datalust.co/docs)

#### Seq

- [x] [Searching and Analyzing Logs](https://docs.datalust.co/docs/the-seq-query-language)

- Logical and comparison operators

If you've worked with SQL you'll immediately notice the familiar operators `and` and `<>`. Seq uses SQL-style operators in expressions.
The comparisons are `=` (equality), `<>` (inequality, "not equal to"), `<`, `<=`, `>`, and `>=` in their usual roles. In search mode, Seq will also accept C-style `==` and `!=`, but these are convenience aliases and are not supported in query mode, so you should prefer the SQL-style versions.
The logical operators are `and`, `or`, and prefix `not`. Just as with the logical operators, C-style `&&`, `||` and prefix `!` are accepted for convenience.

- Properties

`@Level` is a built-in property. This means it isn't part of the log event's "payload" but a special, well-known piece of data that is tracked separately for every event.

There are several built-in properties like `@Level`. The most important are:

1. `@Timestamp`
   — the time an event occurred, stored as an opaque numeric value; every event in Seq must be timestamped

2. `@Message`
— a message associated with the event; event's don't have to have human-readable messages associated with them, but it's much friendlier to read logs with meaningful messages, so Seq encourages it by making`@Message`first-class

3. `@MessageTemplate`
    — structured log sources that support message templates can send the template that produced a log event and Seq will make it available in this built-in property; the message template is like a "type" for a structured event, and Seq computes a numeric
4. `@EventType`
   property based on it to simplify querying events by type

5. ` @Exception`
   — just like messages, log events don't necessarily all have associated exception information or stack traces, but these details are important enough to get their own field, and

6. `@Properties`
   — the generic structured data associated with the event, in a key-value map.

- Strings and text fragments

Remaining in our example expression are two bits of text, the original error message we were searching for, and `Warning`:

```sql
"operation timed out" and @Level <> 'Warning'
```

Despite their similarities, these are different constructs in Seq's query language.

`operation timed out`, double-quoted, is a *text fragment*. It's not compared with anything: it represents some fragment of text that might appear in the event's message (or exception/stack trace). Text fragments are handy for quick searches that need a little more detail: `"operation" and not "timed out"`, for example. Text fragments are always matched in a case-insensitive manner.

`Warning`, with single quotes, is a string. It acts and is used just like any normal programming language string.

- String operations and the `ci` modifier

We've already seen equality and inequality, both of which can be applied to strings. Seq supports more sophisticated comparisons using `like` and `not like`:

```
@Level not like 'Warn%'
```

The `like` operator, borrowed also from SQL, supports `%` (zero-or-more characters) and `_` (one character) wildcards, enabling prefix, suffix, substring, and more complex comparisons.

he universal case-insensitivity modifier `ci` turns whatever operation it is applied to into a case-insensitive one. The `ci` modifier is a postfix operator that works with any string comparison, `=`, `<>`, `like`, `in`, and more:

```
Subsystem = 'smtp' ci
```

Applied to an equality operation like this we'll match values of the `Subsystem` property in any character case: `smtp`, `SMTP`, `Smtp`, etc.

If we go back to our original example, `"operation timed out"`, we can express this using string comparisons as:

```
@Message like '%operation timed out%' ci or
    @Exception like '%operation timed out%' ci
```