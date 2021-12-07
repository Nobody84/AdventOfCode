package day7

import (
	"fmt"
	"log"
	"nobody84/advent-of-code-2021/filesystem"
	"strconv"
	"strings"
)

func Solve() {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines("day7/input.txt")
	if err != nil {
		log.Fatal(err)
	}

	// Get horizontal positions for all craps
	horizontalCrapPositions := getHorizontalCrapPositions(inputLines[0])

	partOne(horizontalCrapPositions)
}

func getHorizontalCrapPositions(inputLine string) (horizontalCrapPositions []uint) {
	splits := strings.Split(inputLine, ",")
	for _, valueString := range splits {
		value, _ := strconv.Atoi(valueString)
		horizontalCrapPositions = append(horizontalCrapPositions, uint(value))
	}

	return
}

// Part One
func partOne(horizontalCrapPositions []uint) {
	min, max := getRange(horizontalCrapPositions)
	horizontalPosition, usedFule := calculateSweetSpot(horizontalCrapPositions, min, max)
	fmt.Println(horizontalPosition, usedFule)

	fmt.Printf("Day 7 - How much fuel must they spend to align to that position? Answer: [%d]\n", usedFule)
}

func getRange(input []uint) (min uint, max uint) {
	max = 0
	min = ^uint(0)
	for i := 0; i < len(input); i++ {
		if input[i] > max {
			max = input[i]
		}

		if input[i] < min {
			min = input[i]
		}
	}

	return
}

func calculateSweetSpot(horizontalCrapPositions []uint, min uint, max uint) (horizontalPosition uint, maxUsedFule uint) {
	maxUsedFule = ^uint(0)
	for i := min; i <= max; i++ {
		usedFule := uint(0)
		for p := 0; p < len(horizontalCrapPositions); p++ {
			if i > horizontalCrapPositions[p] {
				usedFule += (i - horizontalCrapPositions[p])
			} else {
				usedFule += (horizontalCrapPositions[p] - i)
			}

			if usedFule > maxUsedFule {
				break
			}
		}

		if usedFule < maxUsedFule {
			maxUsedFule = usedFule
			horizontalPosition = i
		}
	}

	return
}
