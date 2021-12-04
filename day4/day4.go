package day4

import (
	"fmt"
	"log"
	"strconv"
	"strings"

	"nobody84/advent-of-code-2021/filesystem"
)

var clear map[string]func() //create a map for storing clear funcs
const BoardSize int = 5

type BoardNumber struct {
	value    int
	isMarked bool
}

type BingoBoard struct {
	numbers [5][5]BoardNumber
}

func Solve() {
	// Read input lines
	inputLines, err := filesystem.ReadInputLines("day4/input.txt")
	if err != nil {
		log.Fatal(err)
	}

	// Get drawn numbers
	numbers := getDrawnNumber(inputLines[0])

	// Get boards
	boards := getBoards(inputLines)

	// Part One
	fmt.Print("Day 4 - Part One: What will your final score be if you choose that board? ")

	// Get first winning board
	firstWinningBoard, lastDrawnNumber := getFirstWinningBoard(boards, numbers)
	answer1 := firstWinningBoard.GetScore() * lastDrawnNumber

	fmt.Printf("Answer: [%d]\n", answer1)

	// Part Two
	fmt.Print("Day 4 - Part One: Once it wins, what would its final score be? ")

	lastWinningBoard, lastDrawnNumber := getLastWinningBoard(boards, numbers)
	answer2 := lastWinningBoard.GetScore() * lastDrawnNumber

	fmt.Printf("Answer: [%d]\n", answer2)
}

func getFirstWinningBoard(boards []BingoBoard, numbers []int) (board BingoBoard, lastDrawnNumber int) {
	// Draw numbers
	boardIndex := -1
	lastDrawnNumber = -1
	for _, number := range numbers {
		for i := 0; i < len(boards); i++ {
			boards[i].MarkNumber(number)
			if boards[i].HasBingo() {
				boardIndex = i
				lastDrawnNumber = number
				break
			}
		}

		if boardIndex != -1 {
			break
		}
	}

	return boards[boardIndex], lastDrawnNumber
}

func getLastWinningBoard(boards []BingoBoard, numbers []int) (board BingoBoard, lastDrawnNumber int) {
	finishedBoard := make(map[int]bool)
	boardIndex := -1
	for _, number := range numbers {
		for i := 0; i < len(boards); i++ {
			// Skip board if allready finished.
			_, ok := finishedBoard[i]
			if ok {
				continue
			}

			// Mark the number on the board
			boards[i].MarkNumber(number)
			if boards[i].HasBingo() {
				// If the board is finished, store the index.
				finishedBoard[i] = true
				boardIndex = i
				lastDrawnNumber = number
			}
		}
	}

	return boards[boardIndex], lastDrawnNumber
}

func getDrawnNumber(line string) (drawnNumbers []int) {
	for _, value := range strings.Split(line, ",") {
		number, _ := strconv.Atoi(value)
		drawnNumbers = append(drawnNumbers, number)
	}

	return
}

func getBoards(inputLines []string) (boards []BingoBoard) {
	rowOffset := 2

	for i := rowOffset; i < len(inputLines); i += BoardSize + 1 {
		var newBoard BingoBoard
		for y := 0; y < BoardSize; y++ {
			rowValues := strings.Fields(inputLines[i+y])
			for x := 0; x < BoardSize; x++ {
				number, _ := strconv.Atoi(rowValues[x])
				newBoard.numbers[x][y] = BoardNumber{value: number, isMarked: false}
			}
		}

		boards = append(boards, newBoard)
	}

	return
}

func (board *BingoBoard) Print() {
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

func (board *BingoBoard) HasBingo() (result bool) {
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

func (board *BingoBoard) MarkNumber(number int) {
	for y := 0; y < BoardSize; y++ {
		for x := 0; x < BoardSize; x++ {
			if board.numbers[x][y].value == number {
				board.numbers[x][y].isMarked = true
			}
		}
	}
}

func (board *BingoBoard) GetScore() (sum int) {
	for y := 0; y < BoardSize; y++ {
		for x := 0; x < BoardSize; x++ {
			if !board.numbers[x][y].isMarked {
				sum += board.numbers[x][y].value
			}
		}
	}

	return
}
