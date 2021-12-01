package day1

import (
	"fmt"
	"io"
	"log"
	"os"
)

func readInputNumber(inputFile string) ([]int, error) {
	// Open file
	file, err := os.Open(inputFile)
	if err != nil {
		log.Fatal(err)
	}
	defer file.Close()

	var perline int
	var nums []int

	for {
		_, err := fmt.Fscanf(file, "%d\n", &perline)
		if err != nil {
			if err == io.EOF {
				break // stop reading the file
			}
			fmt.Println(err)
			os.Exit(1)
		}

		nums = append(nums, perline)
	}

	return nums, err
}

func getSummedValues(data []int, windowsSize int) ([]int, error) {
	var result []int
	for i := windowsSize - 1; i < len(data); i++ {
		sum := 0
		for p := 0; p < windowsSize; p++ {
			sum = sum + data[i-p]
		}

		result = append(result, sum)
	}

	return result, nil
}

func getNumberOfPositiveGradients(data []int) (int, error) {
	count := 0
	for i := 1; i < len(data); i++ {
		if data[i-1] < data[i] {
			count++
		}
	}

	return count, nil
}

func Solve() {
	// Read input
	deeps, err := readInputNumber("day1/input.txt")
	if err != nil {
		log.Fatal(err)
	}

	// Part One
	fmt.Print("Day 1 - Part One: How many measurements are larger than the previous measurement? ")

	answer1, _ := getNumberOfPositiveGradients(deeps)
	fmt.Println(fmt.Sprintf("Answer: [%d]", answer1))

	// Part Two
	fmt.Print("Day 1 - Part Two: How many sums are larger than the previous sum? ")

	summedDeeps, _ := getSummedValues(deeps, 3)

	answer2, _ := getNumberOfPositiveGradients(summedDeeps)
	fmt.Println(fmt.Sprintf("Answer: [%d]", answer2))
}
