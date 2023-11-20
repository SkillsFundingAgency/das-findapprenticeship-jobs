Feature: Recruit indexer job

When recruit indexer job is triggered
Then the azure search apprenticeships index is updated

@MockApiClient
Scenario: Live vacancies are retrieved
	Given I invoke the recruit indexer function
	When I have vacancies
	Then they are added to the search index