package day10

import (
	"nobody84/advent-of-code-2021/assert"
	"testing"
)

func TestPartOne(t *testing.T) {
	// Act
	answer := PartOne("input_test.txt")

	// Assert
	assert.Equal(t, answer, 26397, "Day10 - Part One")
}

func TestPartTwo(t *testing.T) {
	// Act
	answer := PartOne("input_test.txt")

	// Assert
	assert.Equal(t, answer, 288957, "Day10 - Part Two")
}
