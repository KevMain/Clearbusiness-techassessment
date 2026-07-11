# Domain Layer - Implementation Details

This document explains key implementation decisions in the Domain layer for future maintainers and technical reviewers.

## CSV Parsing

### Design Choice: Custom Lightweight Parsers
Built custom CSV parsers (`CustomerCsvParser`, `OrderCsvParser`) instead of using a library like CsvHelper.

**Why?** 
- Demonstrates TDD approach from scratch
- Full control over parsing logic and error handling
- Minimal dependencies for a focused domain layer

**Trade-offs**:
- Simple `string.Split(',')` approach - does NOT support quoted fields with commas
- For production with complex CSVs, migrate to CsvHelper

### Parsing Behavior

**Contract**: `IRecordParser<T>.Parse(string csvLine)` returns:
- Domain object on success
- `null` for invalid/header rows

**Field Handling**:
- Whitespace trimmed from all fields
- Required fields: Empty = invalid row
- Optional fields: Empty = null
- Numbers: Culture-invariant parsing

**Date Formats Supported**:
```
dd/MM/yyyy  (e.g., 25/12/2023)
d/M/yyyy    (e.g., 5/3/2023)
yyyy-MM-dd  (e.g., 2023-12-25)
yyyy-M-d    (e.g., 2023-3-5)
```
Falls back to `DateTime.TryParse` if above fail.

**Required vs Optional Dates**:
- `OrderDate` (required) - Invalid parse = row rejected
- `RequiredDate`, `ShippedDate` (optional) - Empty or invalid = null

**Header Detection**:
- First field checked against expected header name (case-insensitive)
- Header rows return `null` and are skipped

### Error Handling

**Current**: Parse returns `null` for failures (malformed rows, invalid data)

**Recommended for Production**:
- Return `Result<T, ParseError>` with line number and error details
- Enables meaningful error reports during imports
- Tests included for current behavior

### Testing

Comprehensive unit tests in `TechnicalAssessment.Domain.Tests` cover:
- Field trimming and validation
- Date format parsing (all supported formats)
- Header detection
- Optional field handling
- Malformed row handling

## Domain Events

### `CustomerDiscountApplied` Event

**Why model as an event?**
- Discount application is a significant business occurrence
- Other systems may need to react (analytics, notifications, audit logs)
- Decouples core discount logic from side effects
- Keeps domain logic focused and testable

**Current Implementation**: In-memory event dispatcher

**Production Evolution Path**:
Replace with message broker (RabbitMQ, Azure Service Bus, Kafka):
1. Publish `CustomerDiscountApplied` to topic/queue
2. Deploy independent subscriber services
3. Scale and retry independently
4. Ensure idempotent subscribers for at-least-once delivery

**Example Subscribers**:
- Analytics service: Update customer metrics
- Notification service: Email/SMS customer about discount
- Audit service: Record discount event for compliance
- Billing service: Adjust invoice calculations

---

## Summary

This layer demonstrates:
- ✅ TDD with comprehensive test coverage
- ✅ Domain-driven design with clear business concepts
- ✅ Extensible event-driven architecture
- ✅ Practical trade-offs documented for reviewers

For architecture overview, see [main README](../README.md).
