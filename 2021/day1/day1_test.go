package day1

import (
	"nobody84/advent-of-code-2021/assert"
	"testing"
)

func TestPartOne(t *testing.T) {
	// Act
	answer := PartOne("input_test.txt")

	// Assert
	assert.Equal(t, answer, 7, "Day1 - Part One")
}

func TestPartTwo(t *testing.T) {
	// Act
	answer := PartTwo("input_test.txt")

	// Assert
	assert.Equal(t, answer, 5, "Day1 - Part Two")
}
