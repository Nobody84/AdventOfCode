package filesystem

import (
	"bufio"
	"os"
)

func ReadInputLines(inputFile string) ([]string, error) {
	// Open file
	file, err := os.Open(inputFile)
	if err != nil {
		return nil, err
	}
	defer file.Close()

	var line string
	var lines []string

	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		// Get line
		line = scanner.Text()
		lines = append(lines, line)
	}

	return lines, nil
}
