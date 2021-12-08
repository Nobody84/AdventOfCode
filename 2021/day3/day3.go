package day3

import (
	"fmt"
	"log"
	"math"
	"strconv"

	"nobody84/advent-of-code-2021/filesystem"
)

type SearchCriteria int

const (
	MostCommonBit SearchCriteria = iota
	LeastCommonBit
)

func Solve() {
	// Read input lines
	reportLines, err := filesystem.ReadInputLines("day3/input.txt")
	if err != nil {
		log.Fatal(err)
	}

	// Part One
	fmt.Print("Day 3 - Part One: What is the power consumption of the submarine? ")

	answer1 := getPowerCosumption(reportLines)
	fmt.Println(fmt.Sprintf("Answer: [%d]", answer1))

	// Part Two
	fmt.Print("Day 3 - Part One: What is the life support rating of the submarine? ")

	answer2 := getLifeSupportRating(reportLines)
	fmt.Println(fmt.Sprintf("Answer: [%d]", answer2))
}

func getPowerCosumption(reportLines []string) (powerConsumption int) {
	numberOfLines := len(reportLines)
	lineLength := len(reportLines[0])

	// Get the number of '1's per collumn
	numberOfOneBitsAtIndex := make([]int, lineLength)
	for x := 0; x < lineLength; x++ {
		numberOfOneBitsAtIndex[x] = getNumberOfOnes(reportLines, x)
	}

	// Get the bits for the gamma and epsilon rate
	gammaRateBits := make([]rune, lineLength)
	for x, value := range numberOfOneBitsAtIndex {
		// 1 is the most common bit of this index if the number of ones
		// is greater than the half of all lines; otherwise 0 is the most common bit.
		if value > numberOfLines-value {
			gammaRateBits[x] = '1'
		} else {
			gammaRateBits[x] = '0'
		}
	}

	// Convert the bit values to int
	gammaRate := convertBitArrayToInt(gammaRateBits)
	epsilonRate := gammaRate ^ int(^uint32(0)>>(32-lineLength))

	return gammaRate * epsilonRate
}

func getLifeSupportRating(reportLines []string) (lifeSupportRating int) {
	// Get the lines for oxygen generator rating and CO2 scrubber rating
	oxigenGeneratorRatingLines := getMatchingLines(reportLines, 0, MostCommonBit)
	co2ScrubberRatingLines := getMatchingLines(reportLines, 0, LeastCommonBit)

	// Convert the bit values to int
	oxygenGeneratorRating := convertBitArrayToInt([]rune(oxigenGeneratorRatingLines[0]))
	co2ScrubberRating := convertBitArrayToInt([]rune(co2ScrubberRatingLines[0]))

	return oxygenGeneratorRating * co2ScrubberRating
}

func getMatchingLines(lines []string, bitPosition int, searchCriteria SearchCriteria) (resultLines []string) {

	numberOfOnes := getNumberOfOnes(lines, bitPosition)
	bitCriteria := "0"
	switch searchCriteria {
	case MostCommonBit:
		if numberOfOnes >= len(lines)-numberOfOnes {
			bitCriteria = "1"
		}
	case LeastCommonBit:
		if numberOfOnes < len(lines)-numberOfOnes {
			bitCriteria = "1"
		}
	}

	var matchingLines []string
	for y := 0; y < len(lines); y++ {
		if string([]rune(lines[y])[bitPosition]) == bitCriteria {
			matchingLines = append(matchingLines, lines[y])
		}
	}

	if len(matchingLines) > 1 {
		return getMatchingLines(matchingLines, bitPosition+1, searchCriteria)
	}

	return matchingLines
}

func getNumberOfOnes(lines []string, bitPosition int) (numberOfOnes int) {
	numberOfOnes = 0
	for y := 0; y < len(lines); y++ {
		if string([]rune(lines[y])[bitPosition]) == "1" {
			numberOfOnes++
		}
	}

	return
}

func convertBitArrayToInt(bitArray []rune) (value int) {
	length := len(bitArray)
	for x := 0; x < length; x++ {
		bitValue, _ := strconv.Atoi(string(bitArray[x]))
		value += bitValue * int(math.Pow(float64(2), float64(length-x-1)))
	}

	return
}
