package day3

import (
	"fmt"
	"log"
	"math"
	"strconv"

	"drueke.biz/advent-of-code-2021/filesystem"
)

func Solve() {
	// Read input lines
	reportLines, err := filesystem.ReadInputLines("day3/input.txt")
	if err != nil {
		log.Fatal(err)
	}

	// Part One
	fmt.Print("Day 3 - Part One: What is the power consumption of the submarine? ")

	gammaRate, epsilonRate := getPowerCosumptionValues(reportLines)
	answer1 := gammaRate * epsilonRate
	fmt.Println(fmt.Sprintf("Answer: [%d]", answer1))
}

func getPowerCosumptionValues(reportLines []string) (gammaRate int, epsilonRate int) {
	numberOfLines := len(reportLines)
	lineLength := len(reportLines[0])

	// Get the number of '1's per collumn
	numberOfOneBitsAtIndex := make([]int, lineLength)
	for y := 0; y < numberOfLines; y++ {
		line := reportLines[y]
		for x, value := range line {
			if string(value) == "1" {
				numberOfOneBitsAtIndex[x]++
			}
		}
	}

	// Get the bits for the gamma and epsilon rate
	gammaRateBits := make([]rune, lineLength)
	epsilonRateBits := make([]rune, lineLength)
	for x, value := range numberOfOneBitsAtIndex {
		// 1 is the most common bit of this index if the number of ones
		// is greater than the half of all lines; otherwise 0 is the most common bit.
		if value > numberOfLines/2 {
			gammaRateBits[x] = '1'
			epsilonRateBits[x] = '0'
		} else {
			gammaRateBits[x] = '0'
			epsilonRateBits[x] = '1'
		}
	}

	// Convert the bit values into int
	gammaRate = 0
	epsilonRate = 0
	for x := 0; x < lineLength; x++ {
		gammaRateBitValue, _ := strconv.Atoi(string(gammaRateBits[x]))
		epsilonRateBitValue, _ := strconv.Atoi(string(epsilonRateBits[x]))
		gammaRate += gammaRateBitValue * int(math.Pow(float64(2), float64(lineLength-x-1)))
		epsilonRate += epsilonRateBitValue * int(math.Pow(float64(2), float64(lineLength-x-1)))
	}

	return gammaRate, epsilonRate
}
