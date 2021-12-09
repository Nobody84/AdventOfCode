package day9

import (
	"log"
	"nobody84/advent-of-code-2021/filesystem"
	"sort"
	"strconv"
)

type Heightmap struct {
	heights [][]int
	width   int
	height  int
}

type Point struct {
	value int
	x     int
	y     int
}

type Basin []Point

func PartOne(puzzleInput string) int {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzleInput)
	if err != nil {
		log.Fatal(err)
	}

	heightMap := getHeightMap(inputLines)
	lowpoints := getLowpoints(heightMap)

	answer := 0
	for _, lowpoint := range lowpoints {
		answer += lowpoint.value + 1
	}

	return answer
}

func PartTwo(puzzleInput string) int {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzleInput)
	if err != nil {
		log.Fatal(err)
	}

	heightMap := getHeightMap(inputLines)
	lowpoints := getLowpoints(heightMap)

	var basins []Basin
	for _, lowpoint := range lowpoints {
		basin := getBasin(heightMap, lowpoint)
		basins = append(basins, basin)
	}

	// Sort
	sort.Slice(basins[:], func(i, j int) bool {
		return len(basins[i]) > len(basins[j])
	})

	// Take the three larges
	return len(basins[0]) * len(basins[1]) * len(basins[2])
}

func getHeightMap(inputsLines []string) (heightmap Heightmap) {
	heightmap.height = len(inputsLines)
	heightmap.heights = make([][]int, heightmap.height)
	for y, line := range inputsLines {
		heightmap.width = len(line)
		heightmap.heights[y] = make([]int, heightmap.width)
		for x, heightString := range line {
			height, _ := strconv.Atoi(string(heightString))
			heightmap.heights[y][x] = height
		}
	}
	return
}

func getLowpoints(heightmap Heightmap) (lowpoints []Point) {
	for y := 0; y < heightmap.height; y++ {
		for x := 0; x < heightmap.width; x++ {
			if y == 0 {
				if x == 0 { // first row, first column
					if heightmap.heights[y][x] < heightmap.heights[y][x+1] &&
						heightmap.heights[y][x] < heightmap.heights[y+1][x] {
						lowpoints = append(lowpoints, Point{value: heightmap.heights[y][x], x: x, y: y})
					}
				} else if x == heightmap.width-1 { // first row, last column
					if heightmap.heights[y][x] < heightmap.heights[y][x-1] &&
						heightmap.heights[y][x] < heightmap.heights[y+1][x] {
						lowpoints = append(lowpoints, Point{value: heightmap.heights[y][x], x: x, y: y})
					}
				} else { // first row
					if heightmap.heights[y][x] < heightmap.heights[y][x-1] &&
						heightmap.heights[y][x] < heightmap.heights[y][x+1] &&
						heightmap.heights[y][x] < heightmap.heights[y+1][x] {
						lowpoints = append(lowpoints, Point{value: heightmap.heights[y][x], x: x, y: y})
					}
				}
			} else if y == heightmap.height-1 {
				if x == 0 { // last row, first column
					if heightmap.heights[y][x] < heightmap.heights[y][x+1] &&
						heightmap.heights[y][x] < heightmap.heights[y-1][x] {
						lowpoints = append(lowpoints, Point{value: heightmap.heights[y][x], x: x, y: y})
					}
				} else if x == heightmap.width-1 { // last row, last column
					if heightmap.heights[y][x] < heightmap.heights[y][x-1] &&
						heightmap.heights[y][x] < heightmap.heights[y-1][x] {
						lowpoints = append(lowpoints, Point{value: heightmap.heights[y][x], x: x, y: y})
					}
				} else { // last row
					if heightmap.heights[y][x] < heightmap.heights[y][x-1] &&
						heightmap.heights[y][x] < heightmap.heights[y][x+1] &&
						heightmap.heights[y][x] < heightmap.heights[y-1][x] {
						lowpoints = append(lowpoints, Point{value: heightmap.heights[y][x], x: x, y: y})
					}
				}
			} else {
				if x == 0 { // last row, first column
					if heightmap.heights[y][x] < heightmap.heights[y][x+1] &&
						heightmap.heights[y][x] < heightmap.heights[y-1][x] &&
						heightmap.heights[y][x] < heightmap.heights[y+1][x] {
						lowpoints = append(lowpoints, Point{value: heightmap.heights[y][x], x: x, y: y})
					}
				} else if x == heightmap.width-1 { // last row, last column
					if heightmap.heights[y][x] < heightmap.heights[y][x-1] &&
						heightmap.heights[y][x] < heightmap.heights[y-1][x] &&
						heightmap.heights[y][x] < heightmap.heights[y+1][x] {
						lowpoints = append(lowpoints, Point{value: heightmap.heights[y][x], x: x, y: y})
					}
				} else {
					if heightmap.heights[y][x] < heightmap.heights[y][x-1] &&
						heightmap.heights[y][x] < heightmap.heights[y][x+1] &&
						heightmap.heights[y][x] < heightmap.heights[y-1][x] &&
						heightmap.heights[y][x] < heightmap.heights[y+1][x] {
						lowpoints = append(lowpoints, Point{value: heightmap.heights[y][x], x: x, y: y})
					}
				}
			}
		}
	}

	return
}

func getBasin(heightmap Heightmap, lowpoint Point) Basin {
	var visitedPoints Basin
	pointsToVisit := []Point{lowpoint}
	for {
		var newNeighbourPoints []Point
		for _, point := range pointsToVisit {
			// Check if point has alread been visited.
			visited := false
			for _, visitedPoint := range visitedPoints {
				if point.x == visitedPoint.x && point.y == visitedPoint.y {
					visited = true
				}
			}

			if visited {
				continue
			}

			visitedPoints = append(visitedPoints, point)

			// Left
			if point.x != 0 {
				if heightmap.IsHeigher(point.y, point.x-1, point.value) {
					newNeighbourPoints = append(newNeighbourPoints, Point{value: heightmap.GetValue(point.y, point.x-1), y: point.y, x: point.x - 1})
				}
			}
			// Top
			if point.y != 0 {
				if heightmap.IsHeigher(point.y-1, point.x, point.value) {
					newNeighbourPoints = append(newNeighbourPoints, Point{value: heightmap.GetValue(point.y-1, point.x), y: point.y - 1, x: point.x})
				}
			}
			// Right
			if point.x != heightmap.width-1 {
				if heightmap.IsHeigher(point.y, point.x+1, point.value) {
					newNeighbourPoints = append(newNeighbourPoints, Point{value: heightmap.GetValue(point.y, point.x+1), y: point.y, x: point.x + 1})
				}
			}
			// Down
			if point.y != heightmap.height-1 {
				if heightmap.IsHeigher(point.y+1, point.x, point.value) {
					newNeighbourPoints = append(newNeighbourPoints, Point{value: heightmap.GetValue(point.y+1, point.x), y: point.y + 1, x: point.x})
				}
			}
		}

		pointsToVisit = newNeighbourPoints

		if len(newNeighbourPoints) == 0 {
			return visitedPoints
		}
	}
}

func (heightmap *Heightmap) GetValue(y int, x int) int {
	return heightmap.heights[y][x]
}

func (heightmap *Heightmap) IsHeigher(y int, x int, value int) bool {
	return heightmap.heights[y][x] != 9 && heightmap.heights[y][x] > value
}
