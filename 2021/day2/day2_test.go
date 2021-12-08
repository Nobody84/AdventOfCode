package day2

import (
	"nobody84/advent-of-code-2021/assert"
	"testing"
)

func TestPartOne(t *testing.T) {
	// Act
	answer := PartOne("input_test.txt")

	// Assert
	assert.Equal(t, answer, 150, "Day2 - Part One")
}

func TestPartTwo(t *testing.T) {
	// Act
	answer := PartTwo("input_test.txt")

	// Assert
	assert.Equal(t, answer, 900, "Day2 - Part Two")
}
