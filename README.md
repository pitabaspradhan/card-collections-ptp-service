# Card Collections - Promise To Pay Service

## Overview

Card Collections is a RESTful API developed using ASP.NET Core and Clean Architecture principles to manage credit card delinquency collection cases and customer Promise-To-Pay (PTP) commitments.

The solution demonstrates:

* Domain Driven Design (DDD)
* Clean Architecture
* REST API Design
* Idempotent API Processing
* Audit Trail Management
* Domain Events
* Dependency Injection
* Global Exception Handling
* Unit Testing

---

## Business Scenario

A customer has an outstanding credit card balance that has become delinquent.

A collection case is created for the customer and remains in **Open** status.

When the customer commits to making a payment on a future date, a **Promise-To-Pay (PTP)** record is created and the case status transitions to **PromiseToPay**.

The system also records an audit trail and supports idempotent request processing to prevent duplicate submissions.

---

## Solution Architecture

### Architecture Style

Clean Architecture

```text
Presentation Layer
        |
        v
Application Layer
        |
        v
Domain Layer
        ^
        |
Infrastructure Layer
```

---
### Responsibilities

- Controllers handle HTTP requests and responses.
- Services contain business logic.
- Repository abstracts persistence.
- Idempotency Store prevents duplicate request processing.
- Event Publisher publishes domain events.
- Domain layer encapsulates business rules and state transitions.

---

## Business Workflow

```text
Collection Agent
      |
      v
Create Collection Case
      |
      v
Case Status = Open
      |
      v
Customer Promises Payment
      |
      v
Create Promise-To-Pay
      |
      v
Validate Request
      |
      v
Check Idempotency
      |
      v
Create PTP Record
      |
      v
Update Case Status
Open -> PromiseToPay
      |
      v
Create Audit Entry
      |
      v
Publish PromiseToPayCreated Event
      |
      v
Return Response
```
### Project Structure

```text
CardCollections.sln

src
│
├── CardCollections.Api
│
├── CardCollections.Application
│
├── CardCollections.Domain
│
└── CardCollections.Infrastructure

tests
│
└── CardCollections.UnitTests

docs
│
└── Architecture-Design-v1.0.md
```

---

## Domain Model

### CollectionCase

Aggregate Root representing a delinquent credit card collection case.

Attributes:

* CaseId
* CustomerId
* MaskedCardNumber
* DelinquentAmount
* Status

Responsibilities:

* Maintain case lifecycle
* Create Promise-To-Pay commitments
* Maintain audit trail

---

### PromiseToPay

Represents a customer commitment to make a payment.

Attributes:

* PromiseToPayId
* Amount
* PromiseDate
* CreatedBy

---

### AuditEntry

Represents audit history for collection activities.

Attributes:

* AuditEntryId
* Action
* UserId
* Timestamp

---

### CaseStatus

Supported statuses:

```text
Open
PromiseToPay
```

---

## Design Decisions

### Aggregate Root

CollectionCase is implemented as the Aggregate Root.

All modifications occur through CollectionCase to ensure consistency.

Example:

```text
CollectionCase
        |
        +-- PromiseToPays
        |
        +-- AuditEntries
```

---

### Idempotency

The Promise-To-Pay endpoint supports idempotent processing.

Purpose:

Prevent duplicate Promise-To-Pay creation due to retries, network failures, or accidental multiple submissions.

Implementation:

```text
Idempotency-Key Header
        |
        v
InMemoryIdempotencyStore
```

Duplicate requests are rejected.

---

### Audit Trail

Every Promise-To-Pay creation generates an audit record.

Purpose:

* Operational Traceability
* Compliance
* Support Investigations
* Business History

---

### Domain Events

The solution publishes domain events when a Promise-To-Pay is created.

Example:

```text
PromiseToPayCreated
```

Current implementation uses an in-memory publisher.

Future implementations may integrate:

* Azure Service Bus
* Kafka
* RabbitMQ

---

## API Endpoints

### Create Collection Case

```http
POST /api/cases
```

Request:

```json
{
  "customerId": "CUST001",
  "maskedCardNumber": "XXXX-XXXX-XXXX-1234",
  "delinquentAmount": 25000
}
```

Response:

```json
{
  "caseId": "guid",
  "status": "Open"
}
```

---

### Get Collection Case

```http
GET /api/cases/{id}
```

Response:

```json
{
  "caseId": "guid",
  "customerId": "CUST001",
  "status": "Open"
}
```

---

### Create Promise-To-Pay

```http
POST /api/cases/{id}/promise-to-pay
```

Headers:

```http
Idempotency-Key: ptp-001
correlationId: trace-001
```

Request:

```json
{
  "amount": 10000,
  "promiseDate": "2026-06-15",
  "agentId": "AGENT001"
}
```

Response:

```json
{
  "promiseToPayId": "guid",
  "caseId": "guid",
  "status": "PromiseToPay"
}
```

---

## Validation Rules

### Collection Case

* CustomerId is required
* MaskedCardNumber is required
* DelinquentAmount must be greater than zero

### Promise-To-Pay

* Amount must be greater than zero
* PromiseDate cannot be in the past
* Collection Case must exist
* Duplicate Idempotency Keys are rejected

---

## Exception Handling

Global Exception Middleware is implemented.

Examples:

### Validation Failure

```json
{
  "error": "CustomerId is required."
}
```

### Duplicate Request

```json
{
  "error": "Duplicate request detected."
}
```

---

## Dependency Injection

Registered Services:

```text
ICollectionCaseRepository

IIdempotencyStore

IEventPublisher

ICollectionCaseService

IPromiseToPayService
```

---

## Testing

Unit tests cover:

### Domain Tests

* CreatePromiseToPay_ShouldChangeStatusToPromiseToPay
* CreatePromiseToPay_ShouldAddPromiseToPay
* CreatePromiseToPay_ShouldCreateAuditEntry

### Application Tests

* CreateCase_ShouldReturnCaseResponse
* CreateCase_ShouldThrowForInvalidAmount

### PromiseToPay Tests

* CreatePromiseToPay_ShouldCreatePromiseToPay
* CreatePromiseToPay_ShouldRejectDuplicateIdempotencyKey
* CreatePromiseToPay_ShouldThrowWhenCaseNotFound

---

## Running the Application

### Prerequisites

* .NET 10 SDK
* Visual Studio 2026

### Run

```bash
dotnet restore

dotnet build

dotnet run
```

Swagger:

```text
https://localhost:<port>/swagger
```

---

## Assumptions

* In-memory persistence is acceptable for the assessment.
* Authentication and authorization are out of scope.
* Single-node deployment is assumed.
* Idempotency store is in-memory and resets on application restart.

---

## Production Evolution

For a production banking platform, this solution could evolve into:

### Collections Service

Manages collection case lifecycle.

### Promise-To-Pay Service

Handles customer payment commitments.

### Audit Service

Maintains immutable audit history.

### Notification Service

Sends SMS, email, and reminder notifications.

### Event Platform

Kafka, Azure Service Bus, or RabbitMQ for event-driven communication.

```text
Collections Service
         |
         v
      Event Bus
     /    |    \
 Audit  Notification Reporting
```

---

## Idempotency Handling

The API uses the Idempotency-Key header to prevent duplicate Promise-To-Pay creation.

Flow:

1. Request arrives with an Idempotency-Key.
2. Store is checked.
3. If the key exists, the previously generated result is returned.
4. Otherwise the request is processed.
5. The result is stored against the key.

Current implementation:

- In-memory dictionary

Production implementation:

- Redis
- Database table with unique constraint
- Distributed cache

---

## Audit Trail

An audit entry is created for significant business actions.

Captured Information:

- Action
- User
- Timestamp
- CorrelationId

Example:

```text
Action        : PromiseToPay Created
User          : AGENT001
Timestamp     : 2026-06-01T10:15:00Z
CorrelationId : trace-001
```

Purpose:

- Compliance
- Traceability
- Troubleshooting
- Regulatory reporting

---

## Event Publishing

Current implementation uses a fake event publisher.

Published Event:

```text
PromiseToPayCreated
```

Purpose:

- Decouple downstream systems
- Enable event-driven architecture
- Support notifications and reporting

---

## Handling Event Publishing Failure

In production, event publishing should use the Transactional Outbox Pattern.

Flow:

1. Save business data.
2. Save event to Outbox table.
3. Commit transaction.
4. Background worker publishes event.
5. Mark event as processed.

Benefits:

- Prevents lost events.
- Ensures eventual consistency.
- Improves reliability.

---

## Security

Authentication is out of scope for this assessment.

Production implementation would include:

- OAuth2
- OpenID Connect (OIDC)
- JWT Tokens
- Role-Based Access Control (RBAC)

Example Roles:

- Collection Agent
- Supervisor
- Administrator

An API Gateway would enforce authentication and authorization policies.

---

## Deployment Strategy

Production deployment would use:

- Kubernetes / OpenShift
- Rolling Deployment
- Blue-Green Deployment
- Readiness Probes
- Liveness Probes
- Horizontal Pod Autoscaling

Benefits:

- Zero downtime deployments
- High availability
- Scalability

---

## Observability

### Logging

Capture:

- CorrelationId
- CaseId
- PromiseToPayId
- AgentId
- Request Outcome

### Metrics

Monitor:

- Promise-To-Pay Created Count
- Request Latency
- Validation Failures
- Event Publishing Failures

### Alerts

Generate alerts for:

- High Error Rates
- Event Publishing Failures
- Increased Response Time
- Availability Issues

---

## Assumptions

- In-memory persistence is sufficient.
- No external message broker.
- No authentication provider.
- Single service deployment.
- One Promise-To-Pay created per request.

---
## Future Enhancements

* SQL Server Persistence
* Entity Framework Core
* Distributed Cache (Redis)
* Message Bus Integration
* OpenTelemetry Tracing
* Correlation ID Propagation
* Authentication & Authorization
* Payment Arrangement Support
* Distributed Idempotency Store
* Containerization (Docker)
* CI/CD Pipeline


## Author

Pitabas Pradhan

Senior Engineering Leader / Technical Architect

20+ Years of Experience in Enterprise Application Development, Distributed Systems, Cloud-Native Architecture, and Financial Services Platforms.

