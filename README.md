## E2E Tests

### ✅ E2E Test 1: Full Study Group Lifecycle for a Single Subject
#### Title: User creates a study group, joins it, checks its listing, then leaves it.

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

* Group is created successfully with subject Math.
* Alice becomes a member.
* Filtered and sorted views show the correct group.
* After leaving, Alice is no longer listed as a member.

### ✅ E2E Test 2: System Prevents Joining Two Study Groups of Same Subject
#### Title: User attempts to join a second group for the same subject.

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

### ✅ E2E Test 3: Multiple Users Across Different Subjects
#### Title: Multiple users create and join groups across distinct subjects without conflicts.

#### Preconditions:

* User “Carlos” and “Dana” are registered and logged in.
* No current groups exist for Chemistry and Physics.

#### Steps:

* Carlos creates Chem Titans for subject Chemistry.
* Dana creates Physics Pioneers for subject Physics.
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

### ✅ E2E Test 4: Group Creation Validation Errors
#### Title: System prevents invalid group creation attempts.

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
