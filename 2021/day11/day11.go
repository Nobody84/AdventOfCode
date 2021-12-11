package day11

import (
	"fmt"
	"log"
	"nobody84/advent-of-code-2021/filesystem"
	"strconv"
)

type DumboOctopuses struct {
	energyLevel            int
	adjacentDumboOctopuses []*DumboOctopuses
	x, y                   int
	lastFlash              int
}

func PartOne(puzzleInput string) int {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzleInput)
	if err != nil {
		log.Fatal(err)
	}

	dumboOctopuseGrid := GetDumboOctpuses(inputLines)

	// Print(dumboOctopuseGrid)
	numberOfFlashes := 0
	for step := 0; step < 100; step++ {
		for y := 0; y < 10; y++ {
			for x := 0; x < 10; x++ {
				dumboOctopuseGrid[y][x].IncreaseEnergyLevel(step)
			}
		}

		for y := 0; y < 10; y++ {
			for x := 0; x < 10; x++ {
				numberOfFlashes += dumboOctopuseGrid[y][x].FlashIfNeededStep(step)
			}
		}

		// fmt.Println()
		// fmt.Println("After step", step+1)
		Print(dumboOctopuseGrid)
	}

	return numberOfFlashes
}

func PartTwo(puzzleInput string) int {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzleInput)
	if err != nil {
		log.Fatal(err)
	}

	dumboOctopuseGrid := GetDumboOctpuses(inputLines)

	step := 0
	for {
		step++
		for y := 0; y < 10; y++ {
			for x := 0; x < 10; x++ {
				dumboOctopuseGrid[y][x].IncreaseEnergyLevel(step)
			}
		}

		for y := 0; y < 10; y++ {
			for x := 0; x < 10; x++ {
				_ = dumboOctopuseGrid[y][x].FlashIfNeededStep(step)
			}
		}

		allOctopusesAreAboutToFlash := true
		for y := 0; y < 10; y++ {
			for x := 0; x < 10; x++ {
				if dumboOctopuseGrid[y][x].energyLevel != 0 {
					allOctopusesAreAboutToFlash = false
				}
			}
		}

		if allOctopusesAreAboutToFlash {
			break
		}
	}

	return step
}

func GetDumboOctpuses(inputLines []string) (dumboOctopuseGrid [][]DumboOctopuses) {
	for y, line := range inputLines {
		var dumboOctopuses []DumboOctopuses
		for x, value := range line {
			energyLevel, _ := strconv.Atoi(string(value))
			dumboOctopuses = append(dumboOctopuses, DumboOctopuses{y: y, x: x, energyLevel: energyLevel, lastFlash: -1})
		}
		dumboOctopuseGrid = append(dumboOctopuseGrid, dumboOctopuses)
	}

	for y := 0; y < 10; y++ {
		for x := 0; x < 10; x++ {
			if y == 0 {
				if x == 0 { // first row, first column
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y][x+1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y+1][x])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y+1][x+1])
				} else if x == 10-1 { // first row, last column
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y][x-1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y+1][x])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y+1][x-1])
				} else { // first row, n column
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y][x-1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y][x+1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y+1][x-1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y+1][x])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y+1][x+1])
				}
			} else if y == 10-1 {
				if x == 0 { // last row, first column
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y][x+1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y-1][x])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y-1][x+1])
				} else if x == 10-1 { // last row, last column
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y][x-1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y-1][x])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y-1][x-1])
				} else { // last row, n column
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y-1][x-1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y-1][x])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y-1][x+1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y][x-1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y][x+1])
				}
			} else {
				if x == 0 { // n row, first column
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y-1][x])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y-1][x+1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y][x+1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y+1][x])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y+1][x+1])
				} else if x == 10-1 { // n row, last column
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y-1][x])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y-1][x-1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y][x-1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y+1][x])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y+1][x-1])
				} else {
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y-1][x-1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y-1][x])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y-1][x+1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y][x-1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y][x+1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y+1][x-1])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y+1][x])
					dumboOctopuseGrid[y][x].adjacentDumboOctopuses = append(dumboOctopuseGrid[y][x].adjacentDumboOctopuses, &dumboOctopuseGrid[y+1][x+1])
				}
			}
		}
	}

	return
}

func (dumboOctopuses *DumboOctopuses) IncreaseEnergyLevel(step int) {
	if dumboOctopuses.lastFlash == step {
		return
	}

	dumboOctopuses.energyLevel++
}

func (dumboOctopuses *DumboOctopuses) FlashIfNeededStep(step int) int {
	if dumboOctopuses.energyLevel <= 9 {
		return 0
	}

	dumboOctopuses.energyLevel = 0
	if dumboOctopuses.lastFlash == step {
		return 0
	}

	dumboOctopuses.lastFlash = step
	numberOfFlashes := 1
	for i := 0; i < len(dumboOctopuses.adjacentDumboOctopuses); i++ {
		dumboOctopuses.adjacentDumboOctopuses[i].IncreaseEnergyLevel(step)
		numberOfFlashes += dumboOctopuses.adjacentDumboOctopuses[i].FlashIfNeededStep(step)
	}

	return numberOfFlashes
}

func Print(dumboOctopuseGrid [][]DumboOctopuses) {
	for y := 0; y < 10; y++ {
		for x := 0; x < 10; x++ {
			fmt.Print(dumboOctopuseGrid[y][x].energyLevel)
		}
		fmt.Println()
	}
}
