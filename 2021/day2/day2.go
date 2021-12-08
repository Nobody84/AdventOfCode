package day2

import (
	"log"
	"nobody84/advent-of-code-2021/filesystem"
	"strconv"
	"strings"
)

type Command struct {
	direction string
	units     int
}

func PartOne(puzzelInput string) int {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzelInput)
	if err != nil {
		log.Fatal(err)
	}

	submarineCommands := getCommands(inputLines)
	depth, horizontalPosition := calcultePosition(submarineCommands)
	return depth * horizontalPosition
}

func PartTwo(puzzelInput string) int {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzelInput)
	if err != nil {
		log.Fatal(err)
	}

	submarineCommands := getCommands(inputLines)
	depth, horizontalPosition := calcultePositionWithAim(submarineCommands)
	return depth * horizontalPosition
}

func getCommands(inputLines []string) (commands []Command) {
	for _, line := range inputLines {
		// Split line
		splits := strings.Split(line, " ")
		direction := splits[0]
		units, _ := strconv.Atoi(splits[1])

		nextCommand := Command{direction: direction, units: units}
		commands = append(commands, nextCommand)
	}

	return commands
}

func calcultePosition(commands []Command) (depth int, horizontalPosition int) {
	depth = 0
	horizontalPosition = 0

	for _, Command := range commands {
		switch Command.direction {
		case "forward":
			horizontalPosition += Command.units
		case "down":
			depth += Command.units
		case "up":
			depth -= Command.units
		}
	}

	return depth, horizontalPosition
}

func calcultePositionWithAim(commands []Command) (depth int, horizontalPosition int) {
	depth = 0
	horizontalPosition = 0
	aim := 0

	for _, Command := range commands {
		switch Command.direction {
		case "forward":
			horizontalPosition += Command.units
			depth += aim * Command.units
		case "down":
			aim += Command.units
		case "up":
			aim -= Command.units
		}
	}

	return depth, horizontalPosition
}
