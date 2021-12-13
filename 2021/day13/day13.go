package day13

import (
	"fmt"
	"log"
	"nobody84/advent-of-code-2021/filesystem"
	"regexp"
	"strconv"
	"strings"
)

type Dot struct {
	y, x int
}

type FoldingInstruction struct {
	axis  string
	value int
}

type TransparentPaper struct {
	dots                []Dot
	foldingInstructions []FoldingInstruction
}

func PartOne(puzzleInput string) int {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzleInput)
	if err != nil {
		log.Fatal(err)
	}

	transparentPaper := ReadPaper(inputLines)
	transparentPaper.FoldOneTime()

	return len(transparentPaper.dots)
}

func PartTwo(puzzleInput string) {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines(puzzleInput)
	if err != nil {
		log.Fatal(err)
	}

	transparentPaper := ReadPaper(inputLines)
	transparentPaper.Fold()
	transparentPaper.Print()

	return
}

func ReadPaper(inputLines []string) (transparentPaper TransparentPaper) {
	// Reader input lines as dot until the line is blank
	var dots []Dot
	row := 0
	for {
		line := inputLines[row]
		row++
		if line == "" {
			break
		}

		splits := strings.Split(line, ",")
		x, _ := strconv.Atoi(splits[0])
		y, _ := strconv.Atoi(splits[1])
		dots = append(dots, Dot{x: x, y: y})
	}

	r := regexp.MustCompile(`fold along (?P<axis>\w{1})=(?P<value>\d+)`)
	// Reader input lines as folding instruction until the end of lines
	var foldingInstructions []FoldingInstruction
	for i := row; i < len(inputLines); i++ {
		line := inputLines[i]
		submatches := r.FindStringSubmatch(line)
		axis := submatches[r.SubexpIndex("axis")]
		value, _ := strconv.Atoi(submatches[r.SubexpIndex("value")])
		foldingInstructions = append(foldingInstructions, FoldingInstruction{axis: axis, value: value})
	}

	return TransparentPaper{dots: dots, foldingInstructions: foldingInstructions}
}

// TransparentPaper extensions
func (transparentPaper *TransparentPaper) Fold() {
	noFoldingInstructions := len(transparentPaper.foldingInstructions)
	for i := 0; i < noFoldingInstructions; i++ {
		transparentPaper.FoldOneTime()
	}
}

func (transparentPaper *TransparentPaper) FoldOneTime() {
	// Get next folding instruction and remove it form the instruction set
	foldingInstruction := transparentPaper.foldingInstructions[0]
	transparentPaper.foldingInstructions = transparentPaper.foldingInstructions[1:]

	existingDots := make(map[string]struct{})
	var resultingDots []Dot
	for _, dot := range transparentPaper.dots {
		var newDot Dot
		if foldingInstruction.axis == "x" && dot.x > foldingInstruction.value {
			newDot = Dot{y: dot.y, x: 2*foldingInstruction.value - dot.x}
		} else if foldingInstruction.axis == "y" && dot.y > foldingInstruction.value {
			newDot = Dot{y: 2*foldingInstruction.value - dot.y, x: dot.x}
		} else {
			newDot = Dot{y: dot.y, x: dot.x}
		}

		if _, ok := existingDots[newDot.ToString()]; !ok {
			resultingDots = append(resultingDots, newDot)
			existingDots[newDot.ToString()] = struct{}{}
		}
	}

	transparentPaper.dots = resultingDots
}

func (transparentPaper *TransparentPaper) Print() {
	width := 0
	height := 0
	for _, dot := range transparentPaper.dots {
		if dot.y > height {
			height = dot.y
		}

		if dot.x > width {
			width = dot.x
		}
	}

	var output [][]string
	for y := 0; y <= height; y++ {
		var line []string
		for x := 0; x <= width; x++ {
			line = append(line, " ")
		}
		output = append(output, line)
	}

	for _, dot := range transparentPaper.dots {
		output[dot.y][dot.x] = "#"
	}

	for y := 0; y <= height; y++ {
		for x := 0; x <= width; x++ {
			fmt.Printf("%s", output[y][x])
		}
		fmt.Println()
	}
}

// Dot extensions
func (dot *Dot) ToString() string {
	return fmt.Sprintf("%d,%d", dot.x, dot.y)
}
