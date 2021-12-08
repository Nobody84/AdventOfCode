package day2

import (
	"bufio"
	"fmt"
	"log"
	"os"
	"strconv"
	"strings"
)

type command struct {
	direction string
	units     int
}

func Solve() {
	submarineCommands, _ := readInputCommands("day2/input.txt")

	// Part One
	fmt.Print("Day 2 - Part One: What do you get if you multiply your final horizontal position by your final depth? ")

	depth, horizontalPosition := calcultePosition(submarineCommands)
	fmt.Printf("Answer: [%d]\n", depth*horizontalPosition)

	// Part Two
	fmt.Print("Day 2 - Part Two: What do you get if you multiply your final horizontal position by your final depth? (With aim) ")

	depth2, horizontalPosition2 := calcultePositionWithAim(submarineCommands)
	fmt.Printf("Answer: [%d]\n", depth2*horizontalPosition2)
}

func readInputCommands(inputFile string) ([]command, error) {
	// Open file
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	var line string
	var commands []command

	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		// Get line
		line = scanner.Text()

		// Split line
		splits := strings.Split(line, " ")
		direction := splits[0]
		units, err := strconv.Atoi(splits[1])
		if err != nil {
			fmt.Println(err)
			os.Exit(1)
		}

		nextCommand := command{direction: direction, units: units}
		commands = append(commands, nextCommand)
	}

	return commands, err
}

func calcultePosition(commands []command) (depth int, horizontalPosition int) {
	depth = 0
	horizontalPosition = 0

	for _, command := range commands {
		switch command.direction {
		case "forward":
			horizontalPosition += command.units
		case "down":
			depth += command.units
		case "up":
			depth -= command.units
		}
	}

	return depth, horizontalPosition
}

func calcultePositionWithAim(commands []command) (depth int, horizontalPosition int) {
	depth = 0
	horizontalPosition = 0
	aim := 0

	for _, command := range commands {
		switch command.direction {
		case "forward":
			horizontalPosition += command.units
			depth += aim * command.units
		case "down":
			aim += command.units
		case "up":
			aim -= command.units
		}
	}

	return depth, horizontalPosition
}
