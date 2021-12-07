package day1

import (
	"log"
	"nobody84/advent-of-code-2021/filesystem"
	"strconv"
)

func PartOne(puzzelInput string) int {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzelInput)
	if err != nil {
		log.Fatal(err)
	}

	depths := getDepth(inputLines)
	return getNumberOfPositiveGradients(depths, 1)
}

func PartTwo(puzzelInput string) int {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzelInput)
	if err != nil {
		log.Fatal(err)
	}

	depths := getDepth(inputLines)
	return getNumberOfPositiveGradients(depths, 3)
}

func getDepth(inputLines []string) (depths []int) {
	for i := 0; i < len(inputLines); i++ {
		depth, _ := strconv.Atoi(inputLines[i])
		depths = append(depths, depth)
	}
	return
}

func getNumberOfPositiveGradients(depths []int, windowsSize int) (result int) {
	result = 0
	for i := 1; i < len(depths)-(windowsSize-1); i++ {
		if sum(depths, i-1, windowsSize) < sum(depths, i, windowsSize) {
			result++
		}
	}
	return
}

func sum(input []int, offset int, windowsSize int) (sum int) {
	for i := 0; i < windowsSize; i++ {
		sum += input[offset+i]
	}
	return
}
