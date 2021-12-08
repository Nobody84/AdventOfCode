package day8

import (
	"log"
	"math"
	"nobody84/advent-of-code-2021/filesystem"
	"nobody84/advent-of-code-2021/stringhelper"
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
	notebookEntries := getNotebookEntries(inputLines)

	return getNumberOf1478(notebookEntries)
}

func PartTwo(puzzleInput string) int {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzleInput)
	if err != nil {
		log.Fatal(err)
	}

	// Get noteboot entries
	notebookEntries := getNotebookEntries(inputLines)

	result := 0
	for _, notebnotebookEntry := range notebookEntries {
		uniqueSignalPatterns := getUniqueSignalPatterns(notebnotebookEntry)
		result += getFourDigitOutputValue(notebnotebookEntry, uniqueSignalPatterns)
	}

	return result
}

func getNotebookEntries(inputLines []string) (notebookEntries []NotebookEntry) {
	for i := 0; i < len(inputLines); i++ {
		splits := strings.Split(inputLines[i], " | ")
		uniqueSignlePatters := strings.Split(splits[0], " ")
		fourDigitOutputValues := strings.Split(splits[1], " ")

		notebookEntry := NotebookEntry{uniqueSignalPatterns: uniqueSignlePatters, fourDigitOutputValues: fourDigitOutputValues}
		notebookEntry.sortValues()
		notebookEntries = append(notebookEntries, notebookEntry)
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

func getUniqueSignalPatterns(notebookEntry NotebookEntry) map[string]int {
	patterns := make(map[int]string, 10)

	for _, uniqueSignalPattern := range notebookEntry.uniqueSignalPatterns {
		noOfDigits := len(uniqueSignalPattern)
		switch noOfDigits {
		case 2: // #1
			patterns[1] = uniqueSignalPattern
		case 3: // #7
			patterns[7] = uniqueSignalPattern
		case 4: // #4
			patterns[4] = uniqueSignalPattern
		case 7: // #8
			patterns[8] = uniqueSignalPattern
		}
	}

	for _, uniqueSignalPattern := range notebookEntry.uniqueSignalPatterns {
		noOfDigits := len(uniqueSignalPattern)
		switch noOfDigits {
		case 6: // #0,6,9
			if len(removeCharsFromString(uniqueSignalPattern, patterns[1])) == 5 {
				patterns[6] = uniqueSignalPattern
			} else if len(removeCharsFromString(uniqueSignalPattern, patterns[4])) == 3 {
				patterns[0] = uniqueSignalPattern
			} else {
				patterns[9] = uniqueSignalPattern
			}
		}
	}

	for _, uniqueSignalPattern := range notebookEntry.uniqueSignalPatterns {
		noOfDigits := len(uniqueSignalPattern)
		switch noOfDigits {
		case 5: // #2,3,5
			if len(removeCharsFromString(uniqueSignalPattern, patterns[1])) == 3 {
				patterns[3] = uniqueSignalPattern
			} else if len(removeCharsFromString(uniqueSignalPattern, patterns[9])) == 0 {
				patterns[5] = uniqueSignalPattern
			} else {
				patterns[2] = uniqueSignalPattern
			}
		}
	}

	uniqueSignalPatternsMap := make(map[string]int, 10)
	for i, pattern := range patterns {
		uniqueSignalPatternsMap[pattern] = i
	}

	return uniqueSignalPatternsMap
}

func getFourDigitOutputValue(notebookEntry NotebookEntry, uniqueSignalPatternsMap map[string]int) (result int) {
	for i, fourDigitOutputValue := range notebookEntry.fourDigitOutputValues {
		result += uniqueSignalPatternsMap[fourDigitOutputValue] * int(math.Pow10(len(notebookEntry.fourDigitOutputValues)-i-1))
	}
	return
}

func (notebookEntry *NotebookEntry) sortValues() {
	for i := 0; i < len(notebookEntry.uniqueSignalPatterns); i++ {
		notebookEntry.uniqueSignalPatterns[i] = stringhelper.SortStringAsc(notebookEntry.uniqueSignalPatterns[i])
	}

	for i := 0; i < len(notebookEntry.fourDigitOutputValues); i++ {
		notebookEntry.fourDigitOutputValues[i] = stringhelper.SortStringAsc(notebookEntry.fourDigitOutputValues[i])
	}
}

func removeCharsFromString(source string, mask string) (result string) {
	result = source
	for _, char := range mask {
		result = strings.Replace(result, string(char), "", 1)
	}
	return
}
