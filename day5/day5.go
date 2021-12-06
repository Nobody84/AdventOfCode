package day5

import (
	"fmt"
	"log"
	"nobody84/advent-of-code-2021/filesystem"
	"strconv"
	"strings"
)

type Point struct {
	x int
	y int
}

type Line struct {
	p1 Point
	p2 Point
}

func Solve() {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines("day5/input.txt")
	if err != nil {
		log.Fatal(err)
	}

	lines := getAlleLines(inputLines)

	// Part One
	fmt.Print("Day 5 - Part One: At how many points do at least two lines overlap? ")

	lineMap1 := getMapPartOne(lines)
	answer1 := 0
	for _, noOfLines := range lineMap1 {
		if noOfLines >= 2 {
			answer1++
		}
	}

	fmt.Printf("Answer: [%d]\n", answer1)

	// Part Two
	fmt.Print("Day 5 - Part One: At how many points do at least two lines overlap? (With diagonal lines) ")

	lineMap2 := getMapPartTwo(lines)
	answer2 := 0
	for _, noOfLines := range lineMap2 {
		if noOfLines >= 2 {
			answer2++
		}
	}

	fmt.Printf("Answer: [%d]\n", answer2)
}

func getAlleLines(inputLines []string) (lines []Line) {
	for i := 0; i < len(inputLines); i++ {
		lines = append(lines, parseToLine(inputLines[i]))
	}

	return
}

func parseToLine(input string) (line Line) {
	coordinateStrings := strings.Split(input, " -> ")

	p1 := strings.Split(coordinateStrings[0], ",")
	line.p1.x, _ = strconv.Atoi(p1[0])
	line.p1.y, _ = strconv.Atoi(p1[1])

	p2 := strings.Split(coordinateStrings[1], ",")
	line.p2.x, _ = strconv.Atoi(p2[0])
	line.p2.y, _ = strconv.Atoi(p2[1])

	return
}

func getMapPartOne(lines []Line) map[Point]int {
	myMap := make(map[Point]int)
	for i := 0; i < len(lines); i++ {
		if lines[i].IsOrthogonal() {
			for _, point := range lines[i].GetPoints() {
				_, exists := myMap[point]
				if !exists {
					myMap[point] = 0
				}

				myMap[point]++
			}
		}
	}

	return myMap
}

func getMapPartTwo(lines []Line) map[Point]int {
	myMap := make(map[Point]int)
	for i := 0; i < len(lines); i++ {
		for _, point := range lines[i].GetPoints() {
			_, exists := myMap[point]
			if !exists {
				myMap[point] = 0
			}

			myMap[point]++
		}
	}

	return myMap
}

func (line *Line) GetPoints() (points []Point) {
	if line.p1.x == line.p2.x { // Vertical lines
		for y := min(line.p1.y, line.p2.y); y <= max(line.p1.y, line.p2.y); y++ {
			points = append(points, Point{x: line.p1.x, y: y})
		}
	} else if line.p1.y == line.p2.y { // Horizontal lines
		for x := min(line.p1.x, line.p2.x); x <= max(line.p1.x, line.p2.x); x++ {
			points = append(points, Point{x: x, y: line.p1.y})
		}
	} else { // Diagonal line
		m := (line.p2.y - line.p1.y) / (line.p2.x - line.p1.x)
		for x := min(line.p1.x, line.p2.x); x <= max(line.p1.x, line.p2.x); x++ {
			y := (m * (x - line.p1.x)) + line.p1.y
			points = append(points, Point{x: x, y: y})
		}
	}

	return
}

func (line *Line) IsOrthogonal() bool {
	return line.p1.x == line.p2.x || line.p1.y == line.p2.y
}

func min(a, b int) int {
	if a < b {
		return a
	}
	return b
}

func max(a, b int) int {
	if a > b {
		return a
	}
	return b
}
