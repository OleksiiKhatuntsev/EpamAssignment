# Unit Tests
## Test Class
StudyGroupTests

### Test Cases
#### TC-U001: Constructor_WithValidParameters_CreatesStudyGroup

Validates that StudyGroup accepts names within the 5-30 character range

#### TC-U002: Constructor_WithMinimumValidNameLength_CreatesStudyGroup

Tests that a study group can be created with a name at the minimum allowed length (5 characters).

#### TC-U003: Constructor_WithMaximumValidNameLength_CreatesStudyGroup

Tests that a study group can be created with a name at the maximum allowed length (30 characters).

#### TC-U004: Constructor_WithNameBelowMinimumLength_ThrowsException

Ensures that creating a study group with a name shorter than 5 characters throws a WrongStudyGroupException.

#### TC-U005: Constructor_WithNameAboveMaximumLength_ThrowsException

Ensures that creating a study group with a name longer than 30 characters throws a WrongStudyGroupException.

#### TC-U006: AddUser_WithValidUser_AddsUserToList

Verifies that the AddUser method successfully adds a user to the study group's user collection.

#### TC-U007: RemoveUser_WithExistingUser_RemovesUserFromList

Tests that the RemoveUser method successfully removes an existing user from the study group's user collection.

## Test Class
StudyGroupController

### Test Cases
#### TC-U008: CreateStudyGroup_WithValidStudyGroup_ReturnsOkResult

Verifies that creating a study group with valid data returns an OK response and calls the repository once.

#### TC-U009: GetStudyGroups_WhenCalled_ReturnsOkObjectResultWithGroups

Tests that retrieving study groups returns an OK response with the expected list of groups from the repository.

#### TC-U010: CreateStudyGroup_WhenRepositoryThrowsException_PropagatesException

Ensures that exceptions thrown by the repository during study group creation are properly propagated to the caller.

#### TC-U011: GetStudyGroups_WhenRepositoryThrowsException_PropagatesException

Confirms that repository exceptions during group retrieval are correctly propagated without being swallowed.

#### TC-U012: JoinStudyGroup_WhenRepositoryThrowsNotFoundException_PropagatesException

Validates that exceptions from the repository when joining a non-existent study group are properly propagated.

#### TC-U013: CreateStudyGroup_WithNullStudyGroup_ReturnsBadRequest

Tests that attempt to create a study group with null input return a BadRequest response.

#### TC-U014: JoinStudyGroup_WithInvalidIds_ShouldValidateInput

Verifies that joining a study group with invalid negative IDs returns a BadRequest response.

#### TC-U015: Constructor_WithNullRepository_ThrowsArgumentNullException

Ensures the controller constructor throws an ArgumentNullException when passed a null repository dependency.

#### TC-U016: SearchStudyGroups_WithEmptySubject_CallsRepositoryWithEmptyString

Tests that searching with an empty subject string correctly pass the empty string to the repository.

#### TC-U017: GetStudyGroups_WhenNoGroupsExist_ReturnsEmptyList

Confirms that when no study groups exist, the controller returns an empty list rather than null or an error.

# Component Tests
## Test Class
StudyGroupControllerTests
Tests the StudyGroupController API endpoints and their integration with the repository layer.

### Test Cases

#### TC-C001: CreateStudyGroup_WithValidStudyGroup_CallRepositoryAndReturnOk

Verifies that creating a valid StudyGroup calls the repository and returns HTTP 200 OK

#### TC-C002: GetStudyGroups_WithValidStudyGroups_ReturnListOfStudyGroups

Tests that the GetStudyGroups endpoint returns a list of StudyGroups with HTTP 200 OK

#### TC-C003: SearchStudyGroups_WithValidGroups_ReturnMatchingGroups

Validates that searching StudyGroups by subject returns filtered results

#### TC-C004: JoinStudyGroup_WithValidGroup_CallRepositoryAndReturnOk

Tests that joining a StudyGroup calls the repository method and returns HTTP 200 OK

#### TC-C005: LeaveStudyGroup_WithValidGroupAndUser_CallRepositoryAndReturnOk

Verifies that leaving a StudyGroup calls the repository method and returns HTTP 200 OK

#### TC-C006: Constructor_WithNullRepository_ThrowsArgumentNullException

Ensures the controller constructor properly validates its dependencies

# E2E Tests

### E2E Test 1: Full Study Group Lifecycle for a Single Subject

#### Preconditions:

- User “Alice” is logged in.
- No existing study group for the subject of Math.

#### Steps:

* Alice navigates to “Create Study Group”.
* She enters:
	+ Name: Math Masters
	+ Subject: Math
* Submits the form.
* System redirects to the Study Group Details page.
* Alice clicks “Join Group”.
* Navigates to the “All Study Groups” page.
* Filters by subject: Math.
* Verifies that Math Masters is listed with her name as a member.
* Sorts the list by “Newest First” and confirms the group appears on top.
* Enters the group again and clicks “Leave Group”.

#### Expected Results:

* The group is created successfully with the subject Math.
* Alice becomes a member.
* Filtered and sorted views show the correct group.
* After leaving, Alice is no longer listed as a member.

### E2E Test 2: System Prevents Joining Two Study Groups of the Same Subject

#### Preconditions:

* User “Bob” is already a member of a Physics study group named Physics Force.
* Another Physics group named Quantum Club exists.

#### Steps:

* Bob opens the Quantum Club study group page.
* Clicks “Join Group”.
* System checks his membership in Physics Force.

#### Expected Result:

* The system blocks the action with an error:
* "You are already in a study group for this subject (Physics)."
* Bob remains in Physics Force only.

### E2E Test 3: Multiple Users Across Different Subjects

#### Preconditions:

* User “Carlos” and “Dana” are registered and logged in.
* No current groups exist for Chemistry and Physics.

#### Steps:

* Carlos creates Chem Titans for the subject of Chemistry.
* Dana creates Physics Pioneers for the subject of Physics.
* Carlos joins Chem Titans.
* Dana joins Physics Pioneers.
* Both view the full study group list.
* They filter by their respective subjects.
* Then sort by “Oldest First”.

#### Expected Results:

* Both groups are created correctly with different subjects.
* Each user is only in one group.
* Filtering and sorting work as expected.
* No errors or cross-subject issues occur.

### E2E Test 4: Group Creation Validation Errors

#### Preconditions:

* User “Eva” is logged in.
* A Math group already exists.

#### Steps:

* Eva attempts to create another group for Math:
* Name: M (1 character)
* Subject: Math
* Submits the form.
* Expected Results:
* Error message appears: "Group name must be between 5–30 characters."
* The form is not submitted.
