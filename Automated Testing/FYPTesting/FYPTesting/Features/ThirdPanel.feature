﻿Feature: Third Panel
# Feature file containing tests for the third panel of the FT Optix project

Background:
	Given the content is refreshed
	Given the tab "Barcode Generator" is open

Scenario Outline: Objects Exist on Page
	Then "<Object>" exists
	Examples:
	| ID | Object                |
	| 1  | Barcode Base Image    |
	| 2  | Barcode Input Field   |

Scenario Outline: Barcode Input Field Validation (Valid)
	When I input "<Input>" into the input field
	Then a barcode for this input is generated
	And the input "<Input>" is displayed under the generated barcode
	Examples:
	| ID | Input            | Comment                 |
	| 1  | H                | Smallest input possible |
	| 2  | 1234567890qwert  | Longest input possible  |
	| 3  | BARCODE          | Regular input           |
	| 4  | qwerty-=[]#;' | Special chars           |


Scenario: Input Field Input Too Long (>15 chars)
	When I input "1234567890qwertyyyyy" into the input field
	Then the input is considered invalid
