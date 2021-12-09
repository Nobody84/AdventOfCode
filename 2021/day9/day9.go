package day9

import (
	"log"
	"nobody84/advent-of-code-2021/filesystem"
	"strconv"
)

type Heightmap struct {
	heights [][]int
	width   int
	height  int
}

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
		answer += lowpoint + 1
	}

	return answer
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

func getLowpoints(heightmap Heightmap) (lowpoints []int) {
	for y := 0; y < heightmap.height; y++ {
		for x := 0; x < heightmap.width; x++ {
			if y == 0 {
				if x == 0 { // first row, first column
					if heightmap.heights[y][x] < heightmap.heights[y][x+1] &&
						heightmap.heights[y][x] < heightmap.heights[y+1][x] {
						lowpoints = append(lowpoints, heightmap.heights[y][x])
					}
				} else if x == heightmap.width-1 { // first row, last column
					if heightmap.heights[y][x] < heightmap.heights[y][x-1] &&
						heightmap.heights[y][x] < heightmap.heights[y+1][x] {
						lowpoints = append(lowpoints, heightmap.heights[y][x])
					}
				} else { // first row
					if heightmap.heights[y][x] < heightmap.heights[y][x-1] &&
						heightmap.heights[y][x] < heightmap.heights[y][x+1] &&
						heightmap.heights[y][x] < heightmap.heights[y+1][x] {
						lowpoints = append(lowpoints, heightmap.heights[y][x])
					}
				}
			} else if y == heightmap.height-1 {
				if x == 0 { // last row, first column
					if heightmap.heights[y][x] < heightmap.heights[y][x+1] &&
						heightmap.heights[y][x] < heightmap.heights[y-1][x] {
						lowpoints = append(lowpoints, heightmap.heights[y][x])
					}
				} else if x == heightmap.width-1 { // last row, last column
					if heightmap.heights[y][x] < heightmap.heights[y][x-1] &&
						heightmap.heights[y][x] < heightmap.heights[y-1][x] {
						lowpoints = append(lowpoints, heightmap.heights[y][x])
					}
				} else { // last row
					if heightmap.heights[y][x] < heightmap.heights[y][x-1] &&
						heightmap.heights[y][x] < heightmap.heights[y][x+1] &&
						heightmap.heights[y][x] < heightmap.heights[y-1][x] {
						lowpoints = append(lowpoints, heightmap.heights[y][x])
					}
				}
			} else {
				if x == 0 { // last row, first column
					if heightmap.heights[y][x] < heightmap.heights[y][x+1] &&
						heightmap.heights[y][x] < heightmap.heights[y-1][x] &&
						heightmap.heights[y][x] < heightmap.heights[y+1][x] {
						lowpoints = append(lowpoints, heightmap.heights[y][x])
					}
				} else if x == heightmap.width-1 { // last row, last column
					if heightmap.heights[y][x] < heightmap.heights[y][x-1] &&
						heightmap.heights[y][x] < heightmap.heights[y-1][x] &&
						heightmap.heights[y][x] < heightmap.heights[y+1][x] {
						lowpoints = append(lowpoints, heightmap.heights[y][x])
					}
				} else {
					if heightmap.heights[y][x] < heightmap.heights[y][x-1] &&
						heightmap.heights[y][x] < heightmap.heights[y][x+1] &&
						heightmap.heights[y][x] < heightmap.heights[y-1][x] &&
						heightmap.heights[y][x] < heightmap.heights[y+1][x] {
						lowpoints = append(lowpoints, heightmap.heights[y][x])
					}
				}
			}
		}
	}

	return
}
