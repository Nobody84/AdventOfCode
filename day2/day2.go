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
	fmt.Println(fmt.Printf("Answer: [%d]", depth*horizontalPosition))
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
			break
		case "down":
			depth += command.units
			break
		case "up":
			depth -= command.units
			break
		}
	}

	return
}
