package day14

import (
	"fmt"
	"log"
	"nobody84/advent-of-code-2021/assert"
	"nobody84/advent-of-code-2021/filesystem"
	"reflect"
	"testing"
)

func TestPartOne(t *testing.T) {
	// Act
	answer := PartOne("input_test.txt")

	// Assert
	assert.Equal(t, answer, 1588, "Day14 - Part One")
}

func TestPartTwo(t *testing.T) {
	// Act
	answer := PartTwo("input.txt")

	// Assert
	assert.Equal(t, answer, 2851, "Day14 - Part Two")
}

func TestExampleSteps(t *testing.T) {

	// Read input lines
	inputLines, err := filesystem.ReadInputLines("input.txt")
	if err != nil {
		log.Fatal(err)
	}

	pairInsertions := getPairInsertions(inputLines[2:])

	polymerTemplate := inputLines[0]
	pairs := make(map[string]int, (len(polymerTemplate)*2)-1)
	for i := 0; i < len(polymerTemplate)-1; i++ {
		pair := polymerTemplate[i : i+2]
		if _, ok := pairs[pair]; !ok {
			pairs[pair] = 1
		} else {
			pairs[pair]++
		}
	}

	fmt.Printf("Pairs: %#v\n", pairs)

	for i := 1; i <= 10; i++ {
		// PartOne
		polymerTemplate = insertPairs(polymerTemplate, pairInsertions)
		expected := CountLettersDebug(polymerTemplate)

		// PartTwo
		pairs = insertPairs2(pairs, pairInsertions)
		answer := CountLetters2Debug(pairs, string(polymerTemplate[len(polymerTemplate)-1]))

		// Assert
		stepOk := reflect.DeepEqual(expected, answer)

		if !stepOk {
			fmt.Printf("Pairs: %#v\n", pairs)
			fmt.Println(polymerTemplate)
			fmt.Printf("Counts exp: %#v\n", expected)
			fmt.Printf("Counts ans: %#v\n", answer)
		}
		assert.True(t, stepOk, fmt.Sprintf("Step %d not ok", i))
	}
}

func CountLetters2Debug(pairs map[string]int, lastLetter string) (letterCounts map[string]int) {
	letterCounts = make(map[string]int)
	letterCounts[lastLetter] = 1
	for pair, count := range pairs {
		if _, ok := letterCounts[string(pair[0])]; !ok {
			letterCounts[string(pair[0])] = count
		} else {
			letterCounts[string(pair[0])] += count
		}
	}
	return
}

func CountLettersDebug(polymerTemplate string) (letterCounts map[string]int) {
	letterCounts = make(map[string]int)
	for _, letter := range polymerTemplate {
		if _, ok := letterCounts[string(letter)]; !ok {
			letterCounts[string(letter)] = 1
		} else {
			letterCounts[string(letter)]++
		}
	}
	return
}
