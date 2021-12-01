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

func PartOne() {
	fmt.Println("Advent of Code Puzzel 1.1")
	fmt.Println("Question: How many measurements are larger than the previous measurement?")

	// Read input
	depths, err := readInputNumber("day1/input.txt")
	if err != nil {
		log.Fatal(err)
	}

	count := 0
	for i := 1; i < len(depths); i++ {
		if depths[i-1] < depths[i] {
			count++
		}
	}

	fmt.Println("Answer:", count)
}

func PartTwo() {
	fmt.Println("Advent of Code Puzzel 1.2")
	fmt.Println("Question: How many sums are larger than the previous sum?")

	// Read input
	depths, err := readInputNumber("day1/input.txt")
	if err != nil {
		log.Fatal(err)
	}

	count := 0
	for i := 3; i < len(depths); i++ {
		sum1 := depths[i-3] + depths[i-2] + depths[i-1]
		sum2 := depths[i-2] + depths[i-1] + depths[i]
		if sum1 < sum2 {
			count++
		}
	}

	fmt.Println("Answer:", count)

}
