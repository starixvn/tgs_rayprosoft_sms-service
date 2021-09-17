Feature: SendShortMessage

Scenario: Send a short message to service
	Given my brandId is "7B96B54B-0A41-4097-9EA6-FF027D9FE044"
	And my phone number is "886988123456"
	When send short message
	Then the request will queued