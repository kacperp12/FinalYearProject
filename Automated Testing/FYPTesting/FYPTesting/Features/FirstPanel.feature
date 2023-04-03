Feature: First Panel
# Feature file containing tests for the first panel of the FT Optix project

Background:
	Given the content is refreshed
	Given the tab "Input Validation" is open

Scenario Outline: Objects Exist on Page
	Then "<Object>" exists
	Examples:
	| ID | Object        |
	| 1  | Eircode Input |
	| 2  | Submit Button |

Scenario Outline: Eircode Input Validation (Valid)
	When I input "<Input>" for Eircode
	And I Press submit
	Then the input is "valid"
	Examples:
	| ID | Input    | Comment        |
	| 1  | D24 FT12 | Standard input |
	| 2  | X99 9999 | Standard input |

Scenario Outline: Eircode Input Validation (Invalid)
	When I input "<Input>" for Eircode
	Then the input is "invalid"
	Examples:
	| ID | Input      | Comment                                              |
	| 1  | H91Y7W8    | No space between routing keys and unique identifiers |
	| 2  | H91        | Invalid number of chars                              |
	| 3  | []+-_@     | Invalid input                                        |
	| 4  | H91 111111 | Too many chars                                       |
	| 5  |            | No chars                                             |
	| 6  | H91 Y7W‰   | Unicode character (per mille sign)                   |

Scenario: Successful Submission Displays Appropriate Message
		When I input "H91 Y7W9" for Eircode
		And I Press submit
		Then a success message is displayed to the user
