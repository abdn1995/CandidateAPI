# Candidate API Documentation

## API Overview

The Candidate API allows for the management of candidate information in a database. It supports operations to create, read, update, and delete candidate data.

### Base URL

Assuming the API is deployed locally, the base URL could be:

`http://localhost:5000/api`

### Endpoints

#### GET `/candidates`

- **Description:** Retrieves a list of all candidates.
- **Response:** `200 OK` with an array of candidates.

#### GET `/candidates/{id}`

- **Description:** Retrieves a candidate by ID.
- **Parameters:** `id` (integer) - the unique ID of the candidate.
- **Response:** `200 OK` with a candidate's details or `404 Not Found` if the candidate does not exist.

#### POST `/candidates`

- **Description:** Adds a new candidate.
- **Request Body:** A candidate object containing first name, last name, email, etc.
- **Response:** `201 Created` with the created candidate's details.

#### PUT `/candidates/{id}`

- **Description:** Updates an existing candidate.
- **Parameters:** `id` (integer) - the unique ID of the candidate.
- **Request Body:** Partial or full candidate data for updates.
- **Response:** `204 No Content` on success or `404 Not Found` if the candidate does not exist.

#### DELETE `/candidates/{id}`

- **Description:** Deletes a candidate by ID.
- **Parameters:** `id` (integer) - the unique ID of the candidate.
- **Response:** `204 No Content` on successful deletion or `404 Not Found` if the candidate does not exist.

### Error Handling

Responses for error cases include proper HTTP status codes, such as `400 Bad Request` for invalid input and `500 Internal Server Error` for any internal issues.

---

## Unit Testing Documentation

### Overview

Unit tests for the Candidate API verify the functionality of the repository and controller layers independently. Tests are performed using xUnit, Moq, and the InMemory database provider of Entity Framework Core.

### Test Setup

- **Repositories:** Tests in `CandidateRepositoryTests.cs` validate data management logic such as adding, updating, and deleting candidates.
- **Controllers:** Tests in `CandidatesControllerTests.cs` ensure that the API endpoints handle requests and responses correctly, including caching logic.

### Key Test Cases

- `GetAllCandidatesAsync_ReturnsAllCandidates`: Ensures that all candidates are retrieved.
- `GetCandidateByIdAsync_ReturnsCorrectCandidate`: Checks that the correct candidate is fetched using the ID.
- `AddCandidateAsync_AddsCandidateSuccessfully`: Verifies that a new candidate is added correctly.
- `DeleteCandidateAsync_DeletesCandidateSuccessfully`: Tests that a candidate is properly deleted.

### Running Tests

To run the tests, navigate to the test directory and use:

`dotnet test`


The results will indicate which tests have passed and which have failed, with detailed outputs for failures.
