package day13

import (
	"nobody84/advent-of-code-2021/assert"
	"testing"
)

func TestPartOne(t *testing.T) {
	// Act
	answer := PartOne("input_test.txt")

	// Assert
	assert.Equal(t, answer, 17, "Day13 - Part One")
}
