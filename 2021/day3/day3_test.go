package day3

import (
	"nobody84/advent-of-code-2021/assert"
	"testing"
)

func TestPartOne(t *testing.T) {
	// Act
	answer := PartOne("input_test.txt")

	// Assert
	assert.Equal(t, answer, 198, "Day3 - Part One")
}

func TestPartTwo(t *testing.T) {
	// Act
	answer := PartTwo("input_test.txt")

	// Assert
	assert.Equal(t, answer, 230, "Day3 - Part Two")
}
