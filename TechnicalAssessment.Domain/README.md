CSV parsing assumptions and behavior
=================================

This document records the parsing assumptions and runtime behavior for the lightweight CSV parsers implemented in the TechnicalAssessment.Domain project (CustomerCsvParser, OrderCsvParser and related helpers).

Purpose
-------
- Capture the decisions made for the TDD exercise so future maintainers understand how malformed or ambiguous input is handled.

General parsing contract
------------------------
- Parsers implement IRecordParser<T> and expose Parse(string csvLine) : T?.
- Parse returns a domain object on success, or null when the row should be ignored or is invalid.
- Parsers are tolerant but conservative: they ignore header rows and treat rows with missing required fields as invalid.

Field handling and mapping
--------------------------
- Fields are split using string.Split(','). The current implementation DOES NOT support quoted fields that contain commas. If the CSV may contain quoted/escaped fields, replace the split logic with a proper CSV reader (CsvHelper) and add tests.
- All string fields are trimmed of leading/trailing whitespace. Empty or whitespace-only fields are treated as null for optional fields and as empty string for required string properties.
- Numeric fields (int) use CultureInfo.InvariantCulture and TryParse semantics. If a required int field cannot be parsed, the row is considered invalid.

Date parsing
------------
- The sample Orders.csv uses date values in dd/MM/yyyy format. The CsvFieldParser.TryParseDateTimeField method supports the following formats (in order):
  - "dd/MM/yyyy"
  - "d/M/yyyy"
  - "yyyy-MM-dd"
  - "yyyy-M-d"
- After attempting the exact formats above, the parser falls back to DateTime.TryParse with CultureInfo.InvariantCulture.
- Required vs optional dates
  - OrderDate (orders) is required. If it cannot be parsed, the row is invalid.
  - RequiredDate and ShippedDate are optional for orders. If the CSV field is empty or cannot be parsed, those properties are set to null.
- For customers, no date fields exist in the provided sample. If you add date fields for other entities, follow the same required/optional policy and document it here.

Header rows
-----------
- Parsers detect header rows by checking the first field for the expected header name (case-insensitive) and return null for header rows so they are ignored during import.

Invalid / malformed rows
------------------------
- When a row is malformed (too few columns), contains an unparsable required integer or required date, or otherwise does not meet the parser's minimal validation rules, Parse returns null. The caller (importer) should collect these nulls and record a structured error if desired.

Phone and optional fields
-------------------------
- Phone fields in Customer are optional and represented as string? (nullable). If empty in the CSV they map to null.

Culture and encoding
---------------------
- Parsing uses CultureInfo.InvariantCulture for numeric and broad date parsing to ensure deterministic behavior across environments.
- Input files are assumed to be UTF-8 encoded without BOM; if you need other encodings, handle that at the file-reading layer.

Limitations & recommended improvements
-------------------------------------
- Quoted/escaped CSV fields (commas inside fields, newlines inside fields) are NOT supported by the simple Split(',') approach. If input can contain such cases, migrate to CsvHelper and adjust tests accordingly.
- Error reporting is currently "null for failures". For production-quality imports, replace Parse's return type with Result<T,ParseError> or a ValidationResult that includes line number, raw text, and parse errors so the importer can provide a useful error report.
- Consider adding a CsvImporter<T> that accepts IRecordParser<T> and returns (List<T> successes, List<ParseError> failures) while streaming file content to avoid high memory usage on large files.

Tests
-----
- Unit tests covering CsvFieldParser, CustomerCsvParser and OrderCsvParser exist under tests/TechnicalAssessment.Domain.Tests and document current behavior (trimming, header detection, date formats, optional date handling, etc.).

If you want a change
--------------------
- To treat optional dates differently (for example treat empty as valid but invalid parse as error), update CsvFieldParser.TryParseDateTimeField or the specific parser to differentiate empty vs invalid content, and add corresponding unit tests.
- To switch from returning null to returning structured errors, I can implement a small Result<T> type and update parsers and tests accordingly — say the word and I will prepare a migration plan and tests.

Document author: GitHub Copilot (on behalf of repository maintainer)

Domain event: CustomerDiscountApplied
-----------------------------------
- Why model as an event: Applying an additional customer discount is a domain-level occurrence that other parts of the system may need to react to (metrics, notifications, audit). Modeling it as an event decouples the discount decision from side-effects and keeps the core domain logic focused and testable.
- Evolving to a real message broker: The current in-memory DomainEvents pattern can be replaced with a durable message broker (e.g., RabbitMQ, Kafka, Azure Service Bus). Instead of invoking in-process handlers, the DiscountEngine would publish a CustomerDiscountApplied message to a topic/queue; subscribers can be deployed, scaled, and retried independently.
- Possible side-effects / subscribers: examples include updating analytics or loyalty points, sending an email or in-app notification to the customer, writing an audit record, or triggering downstream billing adjustments. Subscribers should be idempotent and handle eventual delivery semantics when using a broker.
