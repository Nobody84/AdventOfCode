package day10

import (
	"log"
	"nobody84/advent-of-code-2021/filesystem"
	"sort"
)

var validChuckEnclosurePatterns map[rune]rune

func PartOne(puzzleInput string) int {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzleInput)
	if err != nil {
		log.Fatal(err)
	}

	initializeValidChuckEnclosurePatterns()
	illigalCharacters := getIlligalCharacters(inputLines)

	return calculateScore(illigalCharacters)
}

func PartTwo(puzzleInput string) int {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzleInput)
	if err != nil {
		log.Fatal(err)
	}

	initializeValidChuckEnclosurePatterns()
	var scores []int
	for _, line := range inputLines {
		openChunks := getOpenChunks(line)
		completionCharacters := getCompletionCharacters(openChunks)
		if completionCharacters == nil {
			continue
		}
		score := calculateScore2(completionCharacters)
		scores = append(scores, score)
	}

	sort.Ints(scores)

	return scores[(len(scores) / 2)]
}

func getIlligalCharacters(inputLines []string) (illegalCharacters []rune) {
	var openChunks []rune
	for _, line := range inputLines {
		for _, char := range line {
			if isOpeningChar(char) {
				openChunks = append(openChunks, char)
			}

			if isClosingChar(char) {
				if isMatchingChar(openChunks, char) {
					openChunks = openChunks[:len(openChunks)-1]
				} else {
					illegalCharacters = append(illegalCharacters, char)
					break
				}
			}
		}
	}

	return
}

func getOpenChunks(inputLine string) (openChunks []rune) {
	for _, char := range inputLine {
		if isOpeningChar(char) {
			openChunks = append(openChunks, char)
		}

		if isClosingChar(char) {
			if isMatchingChar(openChunks, char) {
				openChunks = openChunks[:len(openChunks)-1]
			} else {
				return nil
			}
		}
	}

	return
}

func getCompletionCharacters(openChunks []rune) (completionChunks []rune) {
	for i := len(openChunks) - 1; i >= 0; i-- {
		completionChunks = append(completionChunks, validChuckEnclosurePatterns[openChunks[i]])
	}

	return
}

func initializeValidChuckEnclosurePatterns() {
	validChuckEnclosurePatterns = make(map[rune]rune, 4)
	validChuckEnclosurePatterns['('] = ')'
	validChuckEnclosurePatterns['['] = ']'
	validChuckEnclosurePatterns['{'] = '}'
	validChuckEnclosurePatterns['<'] = '>'
}

func calculateScore(illegalCharacters []rune) (score int) {
	score = 0
	for _, char := range illegalCharacters {
		switch char {
		case ')':
			score += 3
		case ']':
			score += 57
		case '}':
			score += 1197
		case '>':
			score += 25137
		}
	}

	return
}

func calculateScore2(completionCharacters []rune) (score int) {
	score = 0
	for _, char := range completionCharacters {
		score = score * 5
		switch char {
		case ')':
			score += 1
		case ']':
			score += 2
		case '}':
			score += 3
		case '>':
			score += 4
		}
	}

	return
}

func isOpeningChar(char rune) bool {
	for k := range validChuckEnclosurePatterns {
		if char == k {
			return true
		}
	}

	return false
	// return char == '(' || char == '[' || char == '{' || char == '>'
}

func isClosingChar(char rune) bool {
	for _, v := range validChuckEnclosurePatterns {
		if char == v {
			return true
		}
	}

	return false
	// return char == ')' || char == ']' || char == '}' || char == '<'
}

func isMatchingChar(lastChars []rune, currentChar rune) bool {
	if len(lastChars) == 0 {
		return false
	}

	return validChuckEnclosurePatterns[lastChars[len(lastChars)-1]] == currentChar
}
