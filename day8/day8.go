package day8

import (
	"log"
	"nobody84/advent-of-code-2021/filesystem"
	"strings"
)

type NotebookEntry struct {
	uniqueSignalPatterns  []string
	fourDigitOutputValues []string
}

func PartOne(puzzleInput string) int {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzleInput)
	if err != nil {
		log.Fatal(err)
	}

	// Get noteboot entries
	getNotebookEntries := getNotebookEntries(inputLines)

	return getNumberOf1478(getNotebookEntries)
}

func getNotebookEntries(inputLines []string) (notebookEntries []NotebookEntry) {
	for i := 0; i < len(inputLines); i++ {
		splits := strings.Split(inputLines[i], " | ")
		uniqueSignlePatters := strings.Split(splits[0], " ")
		fourDigitOutputValues := strings.Split(splits[1], " ")

		notebookEntries = append(notebookEntries, NotebookEntry{uniqueSignalPatterns: uniqueSignlePatters, fourDigitOutputValues: fourDigitOutputValues})
	}
	return
}

func getNumberOf1478(notebookEntries []NotebookEntry) (result int) {
	for _, notebookEntry := range notebookEntries {
		for _, fourDigitOutputValue := range notebookEntry.fourDigitOutputValues {
			noOfDigits := len(fourDigitOutputValue)
			switch noOfDigits {
			case 2: // 1
				result++
			case 3: // 7
				result++
			case 4: // 4
				result++
			case 7: // 8
				result++
			}
		}
	}
	return
}
