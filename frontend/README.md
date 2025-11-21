# Issue Tracker Frontend

A React + TypeScript frontend application demonstrating architectural patterns and best practices.

## Tech Stack

- **React 19** - UI library
- **TypeScript** - Type safety
- **Vite** - Build tool and dev server
- **React Query (TanStack Query)** - Server state management
- **React Hook Form** - Form handling
- **Zod** - Schema validation
- **Tailwind CSS** - Utility-first styling
- **Axios** - HTTP client

## Architecture Overview

This application follows enterprise-level architectural patterns:

### 1. Container/Presentation Pattern
Components are split into smart containers and dumb presentational components:
- **Containers**: Handle business logic, state management, and side effects
- **Presentation**: Pure UI components that receive props and render

### 2. Feature-Based Organization
```
src/
├── components/ui/          # Reusable UI component library
├── features/issues/        # Issue feature module
│   ├── api/               # API client functions
│   ├── components/        # Feature components (Container + View)
│   ├── hooks/             # Feature-specific custom hooks
│   └── types/             # DTOs and validation schemas
├── hooks/                 # Shared custom hooks
├── lib/                   # Utilities and configurations
└── types/                 # Type definitions
    ├── common/           # API types, errors, type guards
    └── domain/           # Business domain models
```

### 3. Custom Hook Composition
Business logic is extracted into reusable custom hooks:
- `usePagination` - Pagination state management
- `useConfirmDialog` - Confirmation dialog abstraction
- `useIssueForm` - Form state and submission logic
- `useIssueListState` - Complex list state management

### 4. Type System Organization
Types are organized by concern:
- **Domain types** (`types/domain/`) - Business models (Issue, IssueStatus)
- **Common types** (`types/common/`) - API types (PaginatedResponse, ApiError)
- **Feature types** (`features/*/types/`) - DTOs and validation schemas

### 5. UI Component Library
Reusable, composable UI components with:
- Variant support (primary, secondary, danger, success)
- Size options (sm, md, lg)
- Error state handling
- Consistent styling via Tailwind

## Key Features

### Separation of Concerns
- **Business logic** is in custom hooks and containers
- **Presentation logic** is in view components
- **API logic** is in dedicated API modules
- **Type definitions** are organized by domain

### Type Safety
- Runtime type validation with type guards
- Custom `ApiError` class for typed error handling
- Separate domain models from DTOs
- Proper use of `import type` for type-only imports

### Best Practices Demonstrated
1. **Single Responsibility Principle** - Each component/hook has one clear purpose
2. **DRY (Don't Repeat Yourself)** - Reusable UI components and hooks
3. **Composition over Inheritance** - Hook composition and component composition
4. **Explicit Dependencies** - Clear import/export structure with barrel exports
5. **Error Handling** - Typed errors with helper methods
6. **Loading States** - Proper loading and error state management
7. **Optimistic Updates** - React Query cache invalidation

## Project Structure

```
frontend/
├── src/
│   ├── components/
│   │   └── ui/                    # UI Component Library
│   │       ├── Button/           # Button with variants
│   │       ├── Card/             # Container card
│   │       ├── Form/             # Form components (Input, TextArea, Select, FormField)
│   │       ├── LoadingSpinner/   # Loading indicator
│   │       └── Pagination/       # Pagination controls
│   │
│   ├── features/
│   │   └── issues/               # Issues Feature Module
│   │       ├── api/
│   │       │   ├── issues.api.ts          # API client functions
│   │       │   └── index.ts               # Barrel export
│   │       │
│   │       ├── components/
│   │       │   ├── IssueList/
│   │       │   │   ├── IssueListContainer.tsx    # Smart component
│   │       │   │   ├── IssueListView.tsx         # Presentation component
│   │       │   │   ├── IssueFilters.tsx          # Filter component
│   │       │   │   └── index.ts
│   │       │   ├── IssueCard/
│   │       │   │   ├── IssueCardContainer.tsx    # Smart component
│   │       │   │   ├── IssueCardView.tsx         # Presentation component
│   │       │   │   └── index.ts
│   │       │   ├── IssueForm/
│   │       │   │   ├── IssueFormView.tsx         # Presentation component
│   │       │   │   ├── index.tsx                 # Wrapper component
│   │       │   │   └── index.ts
│   │       │   └── index.ts
│   │       │
│   │       ├── hooks/
│   │       │   ├── use-issues.ts           # React Query hooks
│   │       │   ├── use-issue-form.ts       # Form logic hook
│   │       │   ├── use-issue-list-state.ts # List state management
│   │       │   └── index.ts
│   │       │
│   │       └── types/
│   │           ├── dtos.ts                 # CreateIssueDto, UpdateIssueDto
│   │           ├── validation.ts           # Zod schemas
│   │           └── index.ts
│   │
│   ├── hooks/
│   │   ├── usePagination.ts              # Pagination state hook
│   │   ├── useConfirmDialog.ts           # Confirmation dialog hook
│   │   └── index.ts
│   │
│   ├── lib/
│   │   ├── api-client.ts                 # Axios instance with interceptors
│   │   ├── query-client.ts               # React Query configuration
│   │   ├── utils/
│   │   │   ├── date.ts                   # Date formatting utilities
│   │   │   ├── status.ts                 # Status utilities
│   │   │   └── index.ts
│   │   └── utils.ts                      # Barrel export
│   │
│   ├── types/
│   │   ├── common/
│   │   │   ├── api.ts                    # ProblemDetails (RFC 9457)
│   │   │   ├── pagination.ts             # PaginatedResponse
│   │   │   ├── errors.ts                 # ApiError class
│   │   │   ├── type-guards.ts            # Runtime validation
│   │   │   └── index.ts
│   │   ├── domain/
│   │   │   ├── issue.ts                  # Issue, IssueStatus
│   │   │   └── index.ts
│   │   └── index.ts
│   │
│   ├── App.tsx                           # Root component
│   ├── main.tsx                          # Application entry point
│   └── index.css                         # Global styles
│
├── package.json
├── tsconfig.json
├── vite.config.ts
└── tailwind.config.js
```

## Component Patterns

### Container/Presentation Example

```typescript
// Container - handles logic and state
export function IssueListContainer() {
  const state = useIssueListState();

  const handleCreateClick = () => {
    state.setIsCreating(true);
  };

  return (
    <IssueListView
      issues={state.issues}
      onCreateClick={handleCreateClick}
      // ... other props
    />
  );
}

// Presentation - pure UI
export function IssueListView({ issues, onCreateClick, ... }: Props) {
  return (
    <div>
      <Button onClick={onCreateClick}>Create Issue</Button>
      {issues.map(issue => <IssueCard key={issue.id} issue={issue} />)}
    </div>
  );
}
```

### Custom Hook Pattern

```typescript
export function useIssueForm({ issue, onSuccess }: Params) {
  const form = useForm({ /* config */ });
  const { create, update } = useIssueOperations();

  const onSubmit = async (data) => {
    if (issue) {
      await update.mutateAsync({ id: issue.id, data });
    } else {
      await create.mutateAsync(data);
    }
    onSuccess();
  };

  return { form, onSubmit, isSubmitting: form.formState.isSubmitting };
}
```

## API Integration

### API Client Configuration
The API client (`lib/api-client.ts`) includes:
- Base URL configuration from environment variables
- Request/response interceptors
- RFC 9457 Problem Details error handling
- Automatic error transformation

### Paginated Endpoints
All list endpoints support pagination:
```typescript
GET /api/issues?status=Open&pageNumber=1&pageSize=20

Response:
{
  "items": [...],
  "pageNumber": 1,
  "pageSize": 20,
  "totalCount": 157,
  "totalPages": 8,
  "hasPrevious": false,
  "hasNext": true
}
```

## Development

### Prerequisites
- Node.js 20.19+ or 22.12+
- npm or yarn

### Setup
```bash
npm install
```

### Development Server
```bash
npm run dev
```

### Build for Production
```bash
npm run build
```

### Type Checking
```bash
npm run build  # TypeScript compilation happens before build
```

### Environment Variables
Create a `.env` file in the frontend directory:
```env
VITE_API_BASE_URL=http://localhost:5000/api
```

## Code Quality

### TypeScript Configuration
- Strict mode enabled
- `verbatimModuleSyntax` for proper type-only imports
- Path aliases configured for cleaner imports


### Component Guidelines
1. **Containers** handle business logic, **Views** handle presentation
2. Extract complex logic into **custom hooks**
3. Use **UI components** from the component library
4. Keep components **focused** - single responsibility
5. Use **TypeScript** for all props and return types

### State Management Strategy
- **Server state**: React Query (useQuery, useMutation)
- **Local UI state**: useState
- **Form state**: React Hook Form

## Testing Strategy

### Component Testing
- **Containers**: Test business logic and state management
- **Views**: Test rendering and user interactions
- **Hooks**: Test in isolation with `renderHook`

### Integration Testing
- Test full user flows (create issue, update issue, etc.)
- Mock API responses at the boundary
- Test error states and loading states

## Performance Optimizations

1. **Code splitting** - Dynamic imports for routes (future)
2. **React Query caching** - Automatic request deduplication
3. **Memoization** - Strategic use in complex components
4. **Virtual scrolling** - For large lists (future enhancement)
5. **Optimistic updates** - Immediate UI feedback

## Accessibility

- Semantic HTML elements
- Proper ARIA labels where needed
- Keyboard navigation support
- Focus management in modals and forms

## Browser Support

- Modern browsers (Chrome, Firefox, Safari, Edge)
- ES2020+ features
- CSS Grid and Flexbox

## Contributing

When adding new features:
1. Follow the **Container/Presentation** pattern
2. Extract business logic to **custom hooks**
3. Use **UI components** from the component library
4. Add proper **TypeScript types**
5. Organize files in the **feature directory**
6. Update **barrel exports** (`index.ts`)

## License

This project is part of a technical assessment.
