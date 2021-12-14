package day14

import (
	"fmt"
	"log"
	"nobody84/advent-of-code-2021/filesystem"
	"sort"
	"strings"
)

type PairInsertions map[string]string

func PartOne(puzzleInput string) int {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzleInput)
	if err != nil {
		log.Fatal(err)
	}

	polymerTemplate := inputLines[0]
	pairInsertions := getPairInsertions(inputLines[2:])

	for i := 0; i < 10; i++ {
		polymerTemplate = insertPairs(polymerTemplate, pairInsertions)
	}

	answer := CountLetters(polymerTemplate)
	return answer
}

func PartTwo(puzzleInput string) int {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzleInput)
	if err != nil {
		log.Fatal(err)
	}

	polymerTemplate := inputLines[0]
	pairInsertions := getPairInsertions(inputLines[2:])

	for i := 0; i < 40; i++ {
		polymerTemplate = insertPairs(polymerTemplate, pairInsertions)
	}

	answer := CountLetters(polymerTemplate)
	return answer
}

func getPairInsertions(inputLines []string) (pairInsertions PairInsertions) {
	pairInsertions = make(map[string]string)
	for _, line := range inputLines {
		splits := strings.Split(line, " -> ")
		pairInsertions[splits[0]] = splits[1]
	}

	return
}

func insertPairs(polymerTemplate string, pairInsertions PairInsertions) (newPolymerTemplate string) {
	templateOffset := 0
	newPolymerTemplate = polymerTemplate[0:1]
	for {
		pair := polymerTemplate[templateOffset : templateOffset+2]
		insertValue, ok := pairInsertions[pair]
		if ok {
			newPolymerTemplate = newPolymerTemplate + fmt.Sprintf("%s%s", insertValue, string(pair[1]))

		}

		templateOffset++
		if templateOffset+1 == len(polymerTemplate) {
			break
		}
	}

	return
}

func CountLetters(polymerTemplate string) int {
	letterCounts := make(map[rune]int)
	for _, letter := range polymerTemplate {
		if _, ok := letterCounts[letter]; !ok {
			letterCounts[letter] = 1
		} else {
			letterCounts[letter]++
		}
	}

	var counts []int
	for _, count := range letterCounts {
		counts = append(counts, count)
	}

	sort.Ints(counts)

	return counts[len(counts)-1] - counts[0]
}
