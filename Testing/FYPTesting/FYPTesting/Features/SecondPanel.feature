Feature: Second Panel

Background:
	Given the content is refreshed
	Given the tab "Button Patterns" is open

Scenario Outline: All Objects Exist
	Then "<Object>" exists
	Examples:
	| ID | Object              |
	| 1  | WarningMinInput     |
	| 2  | WarningMaxInput     |
	| 3  | WarningUpdateButton |
	| 4  | DangerMinInput      |
	| 5  | DangerMaxInput      |
	| 6  | DangerUpdateButton  |
	| 7  | Slider              |
	| 8  | GreenLED            |
	| 9  | OrangeLED           |
	| 10 | RedLED              |

Scenario: Slider Works as Intended
	When I drag the slider to value "50"
	Then the slider value is set to "50"

Scenario Outline: LEDs Flash when Expected with Slider Value Adjustment
	When I drag the slider to value "<SliderValue>"
	Then the "<LEDType>" LED is flashing
	Examples: 
	| ID | SliderValue | LEDType |
	| 1  | 20          | Green   |
	| 2  | 60          | Orange  |
	| 3  | 90          | Red     |

Scenario Outline: Warning Input Box Validation (Valid)
	When I input "<MinValue>" for "Warning" "Min" input box
	* I input "<MaxValue>" for "Warning" "Max" input box
	Then the "Warning" update button is clickable
	Examples:
	| ID | MinValue | MaxValue |
	| 1  | 0        | 10       |
	| 2  | 50       | 51       |
	| 3  | 0        | 100      |

Scenario Outline: Warning Input Box Validation (Invalid)
	When I input "<MinValue>" for "Warning" "Min" input box
	* I input "<MaxValue>" for "Warning" "Max" input box
	Then the "Warning" update button is greyed out
	Examples:
	| ID | MinValue | MaxValue |
	| 1  | 100      | 50       |
	| 2  | 25       | 25       |
	| 3  | -10      | 5        |
	| 4  | 999      | 1100     |
	| 5  | abc      | 5        |
	| 6  | -=}{     | .,$      |

Scenario Outline: Danger Input Box Validation (Valid)
	When I input "<MinValue>" for "Danger" "Min" input box
	* I input "<MaxValue>" for "Danger" "Max" input box
	Then the "Danger" update button is clickable
	Examples:
	| ID | MinValue | MaxValue |
	| 1  | 0        | 10       |
	| 2  | 50       | 51       |
	| 3  | 0        | 100      |

Scenario Outline: Danger Input Box Validation (Invalid)
	When I input "<MinValue>" for "Danger" "Min" input box
	* I input "<MaxValue>" for "Danger" "Max" input box
	Then the "Danger" update button is greyed out
	Examples:
	| ID | MinValue | MaxValue |
	| 1  | 100      | 50       |
	| 2  | 25       | 25       |
	| 3  | -10      | 5        |
	| 4  | 999      | 1100     |
	| 5  | abc      | 5        |
	| 6  | -=}{     | .,$      |

Scenario: Updating Parameters Reflect Changes on Slider
	When I input "<MinValue>" for "<InputType>" "Min" input box
	* I input "<MaxValue>" for "<InputType>" "Max" input box
	* I press the "<InputType>" update button
	Then the "<InputType>" slider parameter range is updated accordingly
	Examples:
	| ID | MinValue | MaxValue | InputType |
	| 1  | 10       | 40       | Warning   |
	| 2  | 60       | 95       | Danger    |
