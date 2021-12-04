package day4

import (
	"fmt"
	"log"
	"regexp"
	"strconv"
	"strings"

	"nobody84/advent-of-code-2021/filesystem"
)

var clear map[string]func() //create a map for storing clear funcs
const BoardSize int = 5

type boardNumber struct {
	value    int
	isMarked bool
}

type bingoBoard struct {
	numbers [5][5]boardNumber
}

func Solve() {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines("day4/input.txt")
	if err != nil {
		log.Fatal(err)
	}

	// Get drawn numbers
	drawnNumber := getDrawnNumber(inputLines[0])

	// Get boards
	boards := getBoards(inputLines)

	// Part One
	fmt.Print("Day 4 - Part One: What will your final score be if you choose that board? ")

	// Draw numbers
	winningBoardIndex := -1
	lastDrawnNumber := -1
	for _, number := range drawnNumber {
		// fmt.Print(number, " ")
		for i := 0; i < len(boards); i++ {
			boards[i].MarkNumber(number)
			// boards[i].Print()
			// fmt.Println()

			if boards[i].HasBingo() {
				// fmt.Println()
				// boards[i].Print()
				winningBoardIndex = i
				lastDrawnNumber = number
				break
			}
		}

		if winningBoardIndex != -1 {
			break
		}
	}

	answer1 := boards[winningBoardIndex].SumUnmarkedNumbers() * lastDrawnNumber
	fmt.Printf("Answer: [%d]\n", answer1)
}

func getDrawnNumber(line string) (drawnNumbers []int) {
	for _, value := range strings.Split(line, ",") {
		number, _ := strconv.Atoi(value)
		drawnNumbers = append(drawnNumbers, number)
	}

	return
}

func getBoards(inputLines []string) (boards []bingoBoard) {
	rowOffset := 2
	whitespaceRegex := regexp.MustCompile(`\s+`)

	for i := rowOffset; i < len(inputLines); i += BoardSize + 1 {
		var newBoard bingoBoard
		for y := 0; y < BoardSize; y++ {
			trimmedLine := whitespaceRegex.ReplaceAllString(strings.TrimSpace(inputLines[i+y]), " ")
			rowValues := strings.Split(trimmedLine, " ")
			for x := 0; x < BoardSize; x++ {
				number, _ := strconv.Atoi(rowValues[x])
				newBoard.numbers[x][y] = boardNumber{value: number, isMarked: false}
			}
		}

		boards = append(boards, newBoard)
	}

	return
}

func (board *bingoBoard) Print() {
	for y := 0; y < BoardSize; y++ {
		for x := 0; x < BoardSize; x++ {
			if board.numbers[x][y].isMarked {
				fmt.Printf("(%2d) ", board.numbers[x][y].value)
			} else {
				fmt.Printf(" %2d  ", board.numbers[x][y].value)
			}
		}

		fmt.Println()
	}
}

func (board *bingoBoard) HasBingo() (result bool) {
	columnResults := make([]bool, BoardSize)
	for i := 0; i < BoardSize; i++ {
		columnResults[i] = true
	}

	for y := 0; y < BoardSize; y++ {
		isWinningRow := true
		for x := 0; x < BoardSize; x++ {
			if !board.numbers[x][y].isMarked {
				isWinningRow = false
				columnResults[x] = false
			}
		}

		if isWinningRow {
			return true
		}
	}

	for _, columnResult := range columnResults {
		if columnResult {
			return true
		}
	}

	return false
}

func (board *bingoBoard) MarkNumber(number int) {
	for y := 0; y < BoardSize; y++ {
		for x := 0; x < BoardSize; x++ {
			if board.numbers[x][y].value == number {
				board.numbers[x][y].isMarked = true
			}
		}
	}
}

func (board *bingoBoard) SumUnmarkedNumbers() (sum int) {
	for y := 0; y < BoardSize; y++ {
		for x := 0; x < BoardSize; x++ {
			if !board.numbers[x][y].isMarked {
				sum += board.numbers[x][y].value
			}
		}
	}

	return
}
