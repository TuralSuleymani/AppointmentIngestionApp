# Architecture & Design Justification

## Architecture Overview

The solution implements a **clean layered architecture** optimized for SaaS products:

- **API Layer** (`AppointmentIngestion.Api`): Handles HTTP concerns, routing, and response formatting
- **Services Layer** (`AppointmentIngestion.Services`): Contains business logic, validation, and orchestration
- **Repository Layer** (`AppointmentIngestion.Domain`): Encapsulates core entities and database interactions

This layering supports **multi-tenancy scalability**, **independent deployment**, and **team autonomy**—critical for SaaS where different teams can own different layers without blocking each other. The clear boundaries enable horizontal scaling (API instances can scale independently from database connections) and facilitate microservice extraction if needed.

## Design Choice: Separate API and Domain Models

**DTOs (Data Transfer Objects)** in the Services layer are deliberately separated from domain entities:

**Benefits:**
- **API Stability**: Domain changes don't break client contracts; we control what's exposed
- **Security**: Prevents over-posting attacks; clients can't manipulate internal domain properties
- **Versioning**: Multiple API versions can map to the same domain model
- **Documentation**: DTOs serve as clear API contracts for frontend teams and external consumers
- **Performance**: DTOs allow projections and can include computed fields without polluting domain entities

For long-term maintainability in agile teams, this separation means backend developers can refactor domain logic aggressively while API contracts remain stable, reducing coordination overhead with frontend teams.

## Testing Philosophy

**Unit tests are prioritized** because they:

1. **Fast Feedback**: Execute in milliseconds, enabling true TDD
2. **Precise Diagnosis**: Failures pinpoint exact logic errors, not infrastructure issues
3. **Refactoring Confidence**: Support aggressive domain model changes without flaky tests
4. **Team Velocity**: Developers run tests locally without database setup overhead
5. **CI/CD Speed**: Build pipelines complete faster, accelerating deployment frequency

Integration tests are valuable but expensive—they're slower, require infrastructure setup, and often fail for environmental reasons unrelated to business logic. In a SaaS context where teams deploy multiple times daily, fast unit test suites are essential for maintaining velocity. We use integration tests selectively for critical paths (e.g., end-to-end appointment creation), not as the primary testing strategy.

## Trade-offs and Omissions

**Intentionally omitted within x-hour constraint:**

- **Comprehensive Logging**: Basic logging exists but lacks correlation IDs, or distributed tracing
- **UI Polish**: Frontend is functional but minimal—no loading states optimization, error boundaries, or accessibility features
- **Authentication/Authorization**: No identity management (would add Auth0/Azure AD B2C in production)
- **Data Validation**: Basic validation only; missing business rule complexity (e.g., appointment conflicts, working hours)
- **Database Migrations**: Used in-memory database; production would need migration strategy and seed data

**Key Trade-off Discussion:**

**Future Scalability Concern: Lack of CQRS Pattern**

Currently, reads and writes use the same models and database path. In a SaaS with growing tenants, this creates **read/write contention**. A fast-moving team would eventually need:

- **CQRS separation**: Write models optimized for consistency, read models optimized for queries
- **Read replicas**: Appointment listings could query denormalized read databases
- **Event sourcing**: Appointment state changes as events enable audit trails and temporal queries

**Why omitted now?** CQRS adds significant complexity (two models, eventual consistency, synchronization) that would consume 3+ hours of the budget. For MVP validation, this premature optimization would slow feature delivery. The current architecture allows CQRS migration later—the repository pattern already abstracts data access, and separate DTOs mean read models can be introduced without breaking API contracts.

**Agile Context Impact:** In a fast-moving team, delaying CQRS until performance metrics demand it (e.g., >1000 appointments/second) follows the "you aren't gonna need it" (YAGNI) principle. The current structure supports gradual refactoring without a big-bang rewrite, maintaining delivery velocity while addressing scalability incrementally.

## Deployment (Extra Credit)

### Containerization Strategy

The application would be containerized using **multi-stage Docker builds** to optimize image size and security:

**Why Docker for SaaS?**
- **Environment Parity**: Eliminates "works on my machine" issues—dev, staging, and production run identical containers
- **Dependency Isolation**: All runtime dependencies packaged together; no server configuration drift
- **Rapid Rollbacks**: Immutable images enable instant rollback to previous versions if deployments fail
- **Cost Efficiency**: Smaller container footprint means cheaper hosting and faster cold starts

**Multi-stage Build Approach:**
1. **Build Stage**: Uses full .NET SDK (600MB+) to compile and publish the application
2. **Runtime Stage**: Uses minimal ASP.NET runtime (200MB) containing only the compiled binaries
3. **Result**: Final production image ~220MB vs ~800MB monolithic image—faster deployments, lower bandwidth costs

**Security Considerations:**
- Run container as non-root user to limit attack surface
- Use official Microsoft base images that receive security patches
- Scan images with Trivy/Snyk in CI pipeline before pushing to registry

### Azure Deployment Architecture

**Infrastructure Components:**

1. **Azure Container Registry (ACR)**
   - Private Docker registry integrated with Azure AD
   - Geo-replication for faster pulls across regions
   - Vulnerability scanning on every image push

2. **Azure Container Apps**
   - Serverless Kubernetes abstraction—no cluster management overhead
   - Auto-scales from 0→N instances based on HTTP load (SaaS cost optimization)
   - Built-in HTTPS certificates and custom domain support
   - Supports blue-green deployments via traffic splitting

3. **Azure SQL Database** (or PostgreSQL)
   - Managed database with automatic backups and point-in-time restore
   - Connection strings stored in Azure Key Vault, injected as environment variables
   - Geo-replication for disaster recovery

4. **Application Insights**
   - Automatic telemetry collection without code changes
   - Real-time performance monitoring and failure alerts
   - Distributed tracing across microservices

5. **Azure Static Web Apps** (Frontend)
   - Global CDN distribution for React app
   - Automatic HTTPS and custom domains
   - API proxying to backend (CORS handled automatically)

### GitHub Actions CI/CD Workflow

**Pipeline Strategy:**

**On Pull Request:**
1.	Run unit tests (fail fast if logic broken)
2.	Build Docker image (verify Dockerfile correctness)
3.	Run security scan on image
4.	Deploy to PR preview environment (isolated Azure Container App instance)
5.	Comment PR with preview URL for QA testing

**On Merge to Main:**
1.	Run full test suite (unit + integration)
2.	Build production Docker image with semantic version tag (e.g., v1.2.3)
3.	Push to Azure Container Registry
4.	Deploy to staging environment
5.	Run smoke tests against staging
6.	Deploy to production with gradual traffic shift (10% → 50% → 100%)
7.	Rollback automatically if error rate spikes


**Workflow Benefits for Fast-Moving Teams:**
- **PR Previews**: Product managers can test features before merge—no waiting for staging deploys
- **Automatic Rollbacks**: Failed deployments don't require manual intervention or 3AM pages
- **Deployment Frequency**: Teams can deploy 10+ times/day without operations bottleneck
- **Audit Trail**: Every production change tied to Git commit and PR discussion

